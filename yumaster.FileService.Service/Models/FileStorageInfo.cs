using System;
using System.Collections.Generic;
using System.Text;
using yumaster.FileService.Db.Entities;

namespace yumaster.FileService.Service.Models
{
    public class FileStorageInfo
    {
        public File File { get; set; }
        public FileOwner Owner { get; set; }

        public Options.Server Server { get; set; }

        public uint PseudoId { get; set; }
    }
}
