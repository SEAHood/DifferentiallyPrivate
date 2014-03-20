using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.DynamicData;
using System.Web.Mvc;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;

namespace DifferentiallyPrivate.Models
{
    public class ASyncService
    {

        public Task<bool> GetTheTruth()
        {
            Thread.Sleep(3000);
            var task = new Task<bool>(() => true);
            task.RunSynchronously();
            return task; 

        }


    }
}
