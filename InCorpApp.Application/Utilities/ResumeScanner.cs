using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InCorpApp.Application.Utilities
{
    public class ResumeScanner
    {
        
        public static bool ParseResume(Stream file, List<string> keywords, string experiencelevel, string experienceYears)
        {
            List<bool> cvscan = new();
            using WordprocessingDocument wordDoc = WordprocessingDocument.Open(file, false);
            if (wordDoc is null)
            {
                return false;
            }
            var body = wordDoc.MainDocumentPart.Document.Body;
            bool checkTagsResult = CheckTags(body, keywords);
            cvscan.Add(checkTagsResult);
            bool checkExperienceResult = CheckExperience(body, experiencelevel);
            cvscan.Add(checkExperienceResult);
            bool checkYearsResul = CheckLevel(body, experienceYears);
            cvscan.Add(checkYearsResul);

            if (cvscan.All(x => x.Equals(true)))
            {
                return true;
            }
            return false;     
                        
        }

        private static bool CheckTags(Body body, List<string> keywords)
        {
            List<bool> foundtags = new();
            var innertext = body.InnerText;
            foreach (var keyword in keywords)
            {
                if (innertext.Contains(keyword))
                {
                    foundtags.Add(true);
                }
            }

            if (foundtags.All(x => x.Equals(true)))
            {
                return true;
            }
            return false;

        }
    
        private static bool CheckExperience(Body body, string experienceLevel)
        {
            return true;
        }

        private static bool CheckLevel(Body body, string level)
        {
            return true;
        }
    }
}
