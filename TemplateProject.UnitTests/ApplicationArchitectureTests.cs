using FluentAssertions;
using NetArchTest.Rules;
using QrAssignment.Application.Abstractions; // ICommand'ın bulunduğu namespace
using System.Reflection;
using Xunit;

namespace TemplateProject.ArchitectureTests;

public class ApplicationArchitectureTests
{
    // Application katmanındaki herhangi bir sınıfı referans alarak o assembly'yi yüklüyoruz
    private static readonly Assembly ApplicationAssembly = typeof(ICommand<>).Assembly;

    [Fact]
    public void Commands_Should_Implement_ICommand()
    {
        // Arrange: Adı "Command" ile biten sınıfları filtrele
        var result = Types.InAssembly(ApplicationAssembly)
            .That()
            .HaveNameEndingWith("Command")
            .And()
            .AreNotInterfaces()
            .And()
            .AreNotAbstract()
            // Act: Bu sınıfların ICommand veya ICommand<T> implemente ettiğini doğrula
            .Should()
            .ImplementInterface(typeof(ICommand<>))
            .GetResult();
         
        if (!result.IsSuccessful && result.FailingTypes != null)
        {
            // FailingTypes içindeki her bir Type nesnesinin Name özelliğini seçiyoruz
            var failingClasses = string.Join(", ", result.FailingTypes.Select(t => t.FullName));

            Assert.Fail($"Şu komut sınıfları ICommand arayüzünü uygulamıyor: {failingClasses}");
        }

        result.IsSuccessful.Should().BeTrue();
    }
}