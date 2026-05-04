using AutoMapper;
using BeatFlowApi.Profiles;
using Xunit;

namespace BeatFlowApi.Tests;

public class MappingTests
{
    [Fact]
    public void AutoMapper_Configuration_IsValid()
    {
        var configuration = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<MappingProfile>();
        });

        configuration.AssertConfigurationIsValid();
    }
}
