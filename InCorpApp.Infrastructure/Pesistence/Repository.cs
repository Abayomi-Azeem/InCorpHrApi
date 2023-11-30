using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using DocumentFormat.OpenXml.Presentation;
using DocumentFormat.OpenXml.Spreadsheet;
using InCorpApp.Application.Abstractions.Persistence;
using InCorpApp.Application.Utilities;
using InCorpApp.Contracts.Admin.GetUnverifiedRecruiters;
using InCorpApp.Contracts.Admin.GetUser;
using InCorpApp.Contracts.Applicant.GetActiveJobs;
using InCorpApp.Contracts.Common;
using InCorpApp.Contracts.Enums;
using InCorpApp.Domain.Dtos;
using InCorpApp.Domain.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace InCorpApp.Infrastructure.Pesistence
{
    public sealed class Repository: IRepository  
    {
        private readonly IAmazonDynamoDB _context;
        private readonly string _tableName;

        public Repository(IAmazonDynamoDB context)
        {
            _context = context;
            _tableName = AWSNames.TableName;
        }

        public async Task<User?> GetById(string email)
        {
            var getItemRequest = new GetItemRequest
            {
                TableName = _tableName,
                Key = new Dictionary<string, AttributeValue>
                {
                    {"Email", new AttributeValue{S = email} }
                }
            };

            var response = await _context.GetItemAsync(getItemRequest);
            if (response.Item.Count == 0)
            {
                return null;
            }
            var itemAsDocument = Document.FromAttributeMap(response.Item);
            return JsonConvert.DeserializeObject<User>(itemAsDocument.ToJson());
        }

        public async Task<bool> InsertAsync(User user)
        {
            var userAsJson = JsonConvert.SerializeObject(user);
            var userAttributes = Document.FromJson(userAsJson).ToAttributeMap();
            var putItemRequest = new PutItemRequest
            {
                TableName = _tableName,
                Item = userAttributes,
                ConditionExpression = "attribute_not_exists(email)"
            };
            var response = await _context.PutItemAsync(putItemRequest);
            return response.HttpStatusCode == System.Net.HttpStatusCode.OK;
        }
    
        public async Task<bool> RemoveAsync(string email)
        {
            var deletedItemRequest = new DeleteItemRequest
            {
                TableName = _tableName,
                Key = new Dictionary<string, AttributeValue>
                {
                    { "Email", new AttributeValue { S = email } },
                }
            };

            var response = await _context.DeleteItemAsync(deletedItemRequest);
            return response.HttpStatusCode == HttpStatusCode.OK;
        }
    
        public async Task<bool> UpdateAsync(User user)
        {
            var userAsJson = JsonConvert.SerializeObject(user);
            var userAttributes = Document.FromJson(userAsJson).ToAttributeMap();
            var putItemRequest = new PutItemRequest
            {
                TableName = _tableName,
                Item = userAttributes,
            };
            var response = await _context.PutItemAsync(putItemRequest);
            return response.HttpStatusCode == System.Net.HttpStatusCode.OK;
        }

        public async Task<IEnumerable<GetUnverifiedRecruitersResponse>> GetUnverifiedRecruiters()
        {
            var users = new List<GetUnverifiedRecruitersResponse>();
            string selectQuery = $"Select * FROM {_tableName} WHERE Role=2 and IsVerified=false";
            var statement = new ExecuteStatementRequest
            {
                Statement = selectQuery,
            };
            var response = await _context.ExecuteStatementAsync(statement);
            foreach (var item in response.Items)
            {
                var itemsAsDocument = Document.FromAttributeMap(item);
                var itemAsJson = JsonConvert.DeserializeObject<User>(itemsAsDocument.ToJson());
                if (itemAsJson is null)
                {
                    continue;
                }
                users.Add(itemAsJson.ToUnverifiedRecruiters());
            }
            return users;
            
        }

        public async Task<IEnumerable<GetUserResponse>> GetUsers(SearchUserBy searchUserBy, string value)
        {
            ExecuteStatementRequest request = new ExecuteStatementRequest();
            var users = new List<GetUserResponse>();
            if (searchUserBy == SearchUserBy.Role)
            {
                string selectQuery = $"Select * FROM {_tableName} WHERE {searchUserBy.ToString()}= {value}";

                request = new ExecuteStatementRequest
                {
                    Statement = selectQuery,
                };
            }
            else
            {
                string query = $"Select * FROM {_tableName} WHERE {searchUserBy.ToString()}= ?";
                var parameters = new List<AttributeValue>
                {
                    new AttributeValue { S = value.ToString() },
                };
                request = new ExecuteStatementRequest
                {
                    Statement = query,
                    Parameters =parameters
                };

            }

            var response = await _context.ExecuteStatementAsync(request);
            foreach (var item in response.Items)
            {
                var itemsAsDocument = Document.FromAttributeMap(item);
                var itemAsJson = JsonConvert.DeserializeObject<User>(itemsAsDocument.ToJson());
                if (itemAsJson is null)
                {
                    continue;
                }
                users.Add(itemAsJson.ToGetUserResponse());
            }
            return users;

        }
    
        public async Task<IEnumerable<User>> GetAllUsers()
        {
            var scanRequest = new ScanRequest
            {
                TableName = _tableName
            };
            var response = await _context.ScanAsync(scanRequest);
            return response.Items.Select(x =>
            {
                var json = Document.FromAttributeMap(x).ToJson();
                return JsonConvert.DeserializeObject<User>(json);
            })!;
        }

        public async Task<IEnumerable<UserExpiredJobs>> GetRecruitersWithExpiredJobs()
        {
            try
            {
                var users = new List<UserExpiredJobs>();
                string selectQuery = $"Select * FROM {_tableName} WHERE Role=2";
                
                var request = new ExecuteStatementRequest
                {
                    Statement = selectQuery,
                };
                var currentDate = DateOnly.FromDateTime(new DateTimeProvider().CurrentDateTime().Date);

                var response = await _context.ExecuteStatementAsync(request);
                foreach (var item in response.Items)
                {
                    var itemsAsDocument = Document.FromAttributeMap(item);
                    var itemAsJson = JsonConvert.DeserializeObject<User>(itemsAsDocument.ToJson());
                    if (itemAsJson is null)
                    {
                        continue;
                    }
                    if (itemAsJson.JobsCreated is not null)
                    {
                        foreach (var job in itemAsJson.JobsCreated)
                        {
                            if (job.ExpirationDate <= currentDate && job.Status != JobStatus.Done.ToString())
                            {
                                var userexpiredJob = new UserExpiredJobs
                                {
                                    user = itemAsJson,
                                    JobId = job.Id,
                                };
                                users.Add(userexpiredJob);
                            }
                        }
                    }                    
                }
                return users;
            }
            catch (Exception ex)
            {
                Console.WriteLine(JsonConvert.SerializeObject(ex));
                throw;
            }

        }
    
        public async Task<IEnumerable<User>> GetApplicantsAppliedJob(string jobPosteremail, Guid jobId)
        {
            try
            {
                var users = new List<User>();
                string selectQuery = $"Select * FROM {_tableName} WHERE Role=1";

                var request = new ExecuteStatementRequest
                {
                    Statement = selectQuery,
                };
                var currentDate = DateOnly.FromDateTime(new DateTimeProvider().CurrentDateTime().Date);

                var response = await _context.ExecuteStatementAsync(request);
                foreach (var item in response.Items)
                {
                    var itemsAsDocument = Document.FromAttributeMap(item);
                    var itemAsJson = JsonConvert.DeserializeObject<User>(itemsAsDocument.ToJson());
                    if (itemAsJson is null)
                    {
                        continue;
                    }
                    if (itemAsJson.JobsCreated is not null)
                    {
                        foreach (var job in itemAsJson.JobsCreated)
                        {
                            if (job.ExpirationDate < currentDate && job.Status != JobStatus.Done.ToString() && job.CurrentJobStage == 1)
                            {
                                users.Add(itemAsJson);
                            }
                        }
                    }
                }
                return users;
            }
            catch (Exception ex)
            {
                Console.WriteLine(JsonConvert.SerializeObject(ex));
                throw;
            }
        }
    
        public async Task<IEnumerable<GetActiveJobsResponse>> GetAllUnExpiredJobs()
        {
            try
            {
                var jobs = new List<GetActiveJobsResponse>();
                string selectQuery = $"Select * FROM {_tableName} WHERE Role=2";

                var request = new ExecuteStatementRequest
                {
                    Statement = selectQuery,
                };
                var currentDate = DateOnly.FromDateTime(new DateTimeProvider().CurrentDateTime().Date);

                var response = await _context.ExecuteStatementAsync(request);
                foreach (var item in response.Items)
                {
                    var itemsAsDocument = Document.FromAttributeMap(item);
                    var itemAsJson = JsonConvert.DeserializeObject<User>(itemsAsDocument.ToJson());
                    if (itemAsJson is null)
                    {
                        continue;
                    }
                    if (itemAsJson.JobsCreated is not null)
                    {
                        foreach (var job in itemAsJson.JobsCreated)
                        {
                            if (job.ExpirationDate > currentDate && job.Status == JobStatus.Active.ToString())
                            {
                                var activeJob = job.ToGetActiveJobs(itemAsJson.Email, itemAsJson.CompanyAddress);
                                jobs.Add(activeJob);
                            }
                        }
                    }
                }
                return jobs;
            }
            catch (Exception ex)
            {
                Console.WriteLine(JsonConvert.SerializeObject(ex));
                throw;
            }
        }
    }
}
