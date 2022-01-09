﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserSkillProfiles.DBServiceLayer
{
    public interface IUserSkillProfileDBDatabaseSettings
    {
        public string UserSkillProfileCollectionName { get; set; }

        public string ConnectionString { get; set; }

        public string DatabaseName { get; set; }
    }
}
