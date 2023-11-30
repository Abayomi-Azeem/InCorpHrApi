using InCorpApp.Contracts.Admin.GetUnverifiedRecruiters;
using InCorpApp.Contracts.Admin.GetUser;
using InCorpApp.Contracts.Applicant.GetActiveJobs;
using InCorpApp.Domain.Dtos;
using InCorpApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InCorpApp.Application.Abstractions.Persistence
{
    public interface IRepository 
    {
        Task<User?> GetById(string email);
        Task<bool> InsertAsync(User user);
        Task<bool> RemoveAsync(string email);
        Task<IEnumerable<GetUnverifiedRecruitersResponse>> GetUnverifiedRecruiters();
        Task<IEnumerable<GetUserResponse>> GetUsers(SearchUserBy searchUserBy, string value);
        Task<bool> UpdateAsync(User user);
        Task<IEnumerable<UserExpiredJobs>> GetRecruitersWithExpiredJobs();
        Task<IEnumerable<GetActiveJobsResponse>> GetAllUnExpiredJobs();
        Task<IEnumerable<User>> GetAllUsers();
    }
}
