using System;
using System.Collections.Generic;
using System.Text;

namespace sweepnet
{
    public enum WorkMode
    {
        RecentChanges=0,
        DatedArticles=1,
        UnreviewedArticles=2,
        List=3,
        Contribution=4,
        None=-1
    }
}
