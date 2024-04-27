namespace Officify.Features.Support;

public class OfficifyPageTest : PageTest
{
    protected FeatureTestSettings Settings => FeatureTestSettings.Instance;

    protected string BaseUrl => Settings.BaseUrl;
}
