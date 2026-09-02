using System.Reflection;
using Xunit;
using QrAssignment.Application.Security;

namespace QrAssignment.Tests.Architecture
{
    public class AuthorizationRegistryTests
    {
        [Fact]
        public void Tum_Commandlar_Kayit_Defterinde_Tanimli_Olmalidir()
        {
            // Projendeki rastgele bir Command üzerinden Assembly'i bul
            var assembly = typeof(QrAssignment.Application.Features.Tenants.Commands.Create.CreateTenantCommand).Assembly;

            var commandTypes = assembly.GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && (t.Name.EndsWith("Command") || t.Name.EndsWith("Query")) && !t.Name.Equals("IdValidationBase"))
                .ToList();

            var unhandledCommands = new List<string>();

            foreach (var type in commandTypes)
            {
                bool isSecured = AuthorizationRegistry.SecuredCommands.ContainsKey(type);
                bool isUnsecured = AuthorizationRegistry.UnsecuredCommands.Contains(type);

                if (!isSecured && !isUnsecured) unhandledCommands.Add(type.Name);
                if (isSecured && isUnsecured) unhandledCommands.Add($"{type.Name} (İki listede birden var!)");
            }

            Assert.True(unhandledCommands.Count == 0,
                "Kayıt defterinde eksik Command'ler bulundu:\n" + string.Join("\n", unhandledCommands));
        }
    }
}