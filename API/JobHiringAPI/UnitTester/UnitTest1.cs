namespace UnitTester
{
    public class UnitTest1
    {
        [Fact]
        public void FundemantalTest()
        {
            switch(1 + 1 == 2)
            {
                case true :
                    Assert.True(1 + 1 == 2);
                    break;
                case false :
                    throw new ThreadStateException();
                    //break;
            }
        }
    }
}