using AutoMapper;
using Moq;
using QrAssignment.Application.Features.QrLocations.Commands.Create;
using QrAssignment.Application.Repositories;
using QrAssignment.Domain.Entity;
using System.Timers;
using Xunit;
// using FluentAssertions; // Tercihe bağlı eklenebilir

namespace TemplateProject.UnitTests.QrAssignment
{
    public class CreateQrLocationCommandHandlerTests
    {
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IQrLocationRepository> _repositoryMock;
        private readonly CreateQrLocationCommandHandler _handler;

        public CreateQrLocationCommandHandlerTests()
        {
            // Ortak kurulum (Setup) işlemleri burada yapılır
            _mapperMock = new Mock<IMapper>();
            _repositoryMock = new Mock<IQrLocationRepository>();

            // Test edeceğimiz gerçek sınıfı, sahte (mock) bağımlılıklarla ayağa kaldırıyoruz
            _handler = new CreateQrLocationCommandHandler(
                _repositoryMock.Object,
                _mapperMock.Object
            );
        }

        [Fact]
        public async Task Handle_Should_CallRepository_And_ReturnSuccessResultWithGuid()
        {
            // ==========================================================
            // 1. ARRANGE (HAZIRLIK)
            // ==========================================================
            var command = new CreateQrLocationCommand
            {
                Name = "Test",
                LocationName = "Kullanıcı", 
                //ParentLocationId = Guid.NewGuid()
            };

            var expectedId = Guid.NewGuid();

            // Mapper'ın döneceği sahte Entity'yi hazırlıyoruz
            var mappedEntity = new QrLocation { Id = expectedId, Name = "Test" };

            // Sisteme şunu öğretiyoruz: "Eğer Mapper.Map çağrılırsa, benim hazırladığım mappedEntity'yi dön!"
            _mapperMock
                .Setup(m => m.Map<QrLocation>(command))
                .Returns(mappedEntity);

            // ==========================================================
            // 2. ACT (EYLEM / ÇALIŞTIRMA)
            // ==========================================================
            var result = await _handler.Handle(command, CancellationToken.None);

            // ==========================================================
            // 3. ASSERT (DOĞRULAMA)
            // ==========================================================

            // A. Sonuç başarılı mı dönüyor?
            Assert.True(result.IsSuccess);

            // B. İçindeki değer, bizim entity'nin Id'sine eşit mi?
            Assert.Equal(expectedId, result.Value);

            // C. EN KRİTİK KONTROL: Repository'nin AddAsync metodu, 
            // bizim sahte entity'miz ile tam olarak 1 kere çağrıldı mı?
            _repositoryMock.Verify(
                r => r.AddAsync(mappedEntity, It.IsAny<CancellationToken>()),
                Times.Once
            );
        }
    }
}