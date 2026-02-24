using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTester
{
    public class FundamentalUnitTest
    {
        [Fact]
        public void FundemantalTest()
        {
            switch (1 + 1 == 2)
            {
                case true:
                    Assert.True(1 + 1 == 2);
                    break;
                case false:
                    throw new ThreadStateException();
                    //break;
            }
        }
    }
}
