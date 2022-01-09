using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserSkillProfiles.DBServiceLayer
{
    public class UserSkillProfileDBDatabaseSettings: IUserSkillProfileDBDatabaseSettings
    {
        public string UserSkillProfileCollectionName { get; set; }

        public string ConnectionString { get; set; }

        public string DatabaseName { get; set; }

    }
}
