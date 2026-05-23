namespace Wizdle.Web.Functional.Tests.Hooks;

using System.Threading.Tasks;

using Reqnroll;
using Reqnroll.BoDi;

[Binding]
internal static class ContainerHook
{
    [BeforeTestRun]
    public static async Task CreateWizdleContainers(IObjectContainer objectContainer)
    {
        ContainerHandle handle = await new WizdleWebContainerBuilder()
            .BuildAsync();

        objectContainer.RegisterInstanceAs<IContainerHandle>(handle);
    }
}
