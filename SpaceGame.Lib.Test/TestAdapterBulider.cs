namespace SpaceGame.Lib.Test;
using Hwdtech;
using Hwdtech.Ioc;
using Xunit.Abstractions;


public class TestAdapterBulider
{
    private readonly ITestOutputHelper output;
    public TestAdapterBulider(ITestOutputHelper output)
    {
        this.output = output;
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<ICommand>("Scopes.Current.Set",
            IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))
        ).Execute();
    }

    [Fact]
    public void SuccessfullTest()
    {
        IoC.Resolve<ICommand>("IoC.Register", "Game.Bulider.Adapter", (object[] args) => new GetGameBuliderAdapterStrategy().Invoke(args)).Execute();
        IoC.Resolve<ICommand>("IoC.Register", "Game.Adapter.Code", (object[] args) => new CreateGameAdapterCodeStrategy().Invoke(args)).Execute();

        var typeOld = typeof(IUObject);
        var typeNew = typeof(IRotatable);

        var result = IoC.Resolve<string>("Game.Adapter.Code", typeOld, typeNew);

        var expected = @"public class IRotatableAdapter : IRotatable {

        IUObject _obj;
    
        public IRotatableAdapter(IUObject obj) => _obj = obj;
    
        public Angle Direction
        {
        
            get
            {
                return IoC.Resolve<Angle>(""Game.Get.Property"", ""Direction"", _obj);
            }
        
            set
            {
                return IoC.Resolve<ICommand>(""Game.Set.Property"", ""Direction"", _obj, value).Execute();
            }
        }
        
        public Angle AngularVelocity
        {
        
            get
            {
                return IoC.Resolve<Angle>(""Game.Get.Property"", ""AngularVelocity"", _obj);
            }
        
        }
        
    }";
        //output.WriteLine(result);
        Assert.Equal(expected, result);
    }
}
