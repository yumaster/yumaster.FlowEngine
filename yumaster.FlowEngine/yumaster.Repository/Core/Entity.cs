using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace yumaster.Repository.Core
{
    public abstract class Entity
    {
        [Browsable(false)]
        public string Id { get; set; }

        public Entity()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}
