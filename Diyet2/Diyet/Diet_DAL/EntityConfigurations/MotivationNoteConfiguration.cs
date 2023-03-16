﻿using Diet_Model.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diet_DAL.EntityConfigurations
{
    public class MotivationNoteConfiguration: EntityTypeConfiguration<MotivationNote>
    {
        public MotivationNoteConfiguration()
        {
            //HasKey(a => a.ID);
            Property(a => a.Text).HasMaxLength(1000).IsRequired();
        }
    }
}
