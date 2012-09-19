using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
namespace Procesta.CvServer
{
    public class SnapShotTimeCollections : ObservableCollection<ModelSnapShotTime>
    {
        public SnapShotTimeCollections()
        {
            this.Add(new ModelSnapShotTime { Name = "30 Seconds", shotTime = new TimeSpan(0, 0,30) });
            this.Add(new ModelSnapShotTime { Name = "1 Minutes",  shotTime = new TimeSpan(0,1,0) });
            this.Add(new ModelSnapShotTime { Name = "5 Minutes",  shotTime = new TimeSpan(0, 5, 0) });
            this.Add(new ModelSnapShotTime { Name = "10 Minutes", shotTime = new TimeSpan(0, 10, 0) });
            this.Add(new ModelSnapShotTime { Name = "20 Minutes", shotTime = new TimeSpan(0, 20, 0) });
            this.Add(new ModelSnapShotTime { Name = "30 Minutes", shotTime = new TimeSpan(0, 30, 0) });
            this.Add(new ModelSnapShotTime { Name = "1 Hour", shotTime = new TimeSpan(1, 0, 0) });
            this.Add(new ModelSnapShotTime { Name = "2 Hours", shotTime = new TimeSpan(2, 0, 0) });
        }
    }
}
