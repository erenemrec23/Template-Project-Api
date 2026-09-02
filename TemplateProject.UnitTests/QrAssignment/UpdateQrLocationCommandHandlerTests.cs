using AutoMapper;
using Moq;
using QrAssignment.Application.Features.QrLocations.Commands.Update;
using QrAssignment.Application.Interfaces;
using QrAssignment.Application.Repositories;
using QrAssignment.Domain.Entity; // QrLocation'ın bulunduğu namespace
using Xunit;

namespace TemplateProject.UnitTests.QrAssignment
{
    public class UpdateQrLocationCommandHandlerTests
    {
        private readonly Mock<IQrLocationRepository> _repositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IAppLocalizer> _localizerMock;
        private readonly UpdateQrLocationCommandHandler _handler;

        public UpdateQrLocationCommandHandlerTests()
        {
            _repositoryMock = new Mock<IQrLocationRepository>();
            _mapperMock = new Mock<IMapper>();
            _localizerMock = new Mock<IAppLocalizer>();

            _handler = new UpdateQrLocationCommandHandler(
                _repositoryMock.Object,
                _mapperMock.Object,
                _localizerMock.Object
            );
        }

        [Fact]
        public async Task Handle_WhenIdIsNull_ShouldThrowException()
        {
            // 1. ARRANGE
            var command = new UpdateQrLocationCommand { Id = null, Name = "Updated Location" };
            var expectedErrorMessage = "Id cannot be null";

            // Localizer'ın döneceği sahte çeviri metnini ayarlıyoruz
            _localizerMock
                .Setup(l => l["Messages.IdIsNull"])
                .Returns(expectedErrorMessage);

            // 2. ACT & 3. ASSERT
            // Handler'ın hata fırlatmasını beklediğimizi söylüyoruz ve dönen hatayı yakalıyoruz
            var exception = await Assert.ThrowsAsync<Exception>(() =>
                _handler.Handle(command, CancellationToken.None));

            Assert.Equal(expectedErrorMessage, exception.Message);

            // Repository'nin HİÇ çağrılmadığından emin oluyoruz (Çünkü daha validasyonda patlamalı)
            _repositoryMock.Verify(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Handle_WhenLocationNotFound_ShouldThrowException()
        {
            // 1. ARRANGE
            var command = new UpdateQrLocationCommand { Id = Guid.NewGuid(), Name = "Updated Location" };
            var expectedErrorMessage = "Location not found";

            // Repository'nin veritabanında kaydı bulamadığını (null döndüğünü) simüle ediyoruz
            _repositoryMock
                .Setup(r => r.GetByIdAsync(command.Id.Value, It.IsAny<CancellationToken>()))
                .ReturnsAsync((QrLocation)null!); // Null döner

            _localizerMock
                .Setup(l => l["Messages.QrLocationNotFound"])
                .Returns(expectedErrorMessage);

            // 2. ACT & 3. ASSERT
            var exception = await Assert.ThrowsAsync<Exception>(() =>
                _handler.Handle(command, CancellationToken.None));

            Assert.Equal(expectedErrorMessage, exception.Message);

            // Update metodunun kesinlikle çalışmadığını doğruluyoruz
            _repositoryMock.Verify(r => r.Update(It.IsAny<QrLocation>()), Times.Never);
        }

        [Fact]
        public async Task Handle_WhenValidRequest_ShouldUpdateAndReturnSuccessResult()
        {
            // 1. ARRANGE
            var command = new UpdateQrLocationCommand { Id = Guid.NewGuid(), Name = "Updated Location" };
            var existingEntity = new QrLocation { Id = command.Id.Value , Name = "Original Location" };

            // Veritabanında kaydın bulunduğunu simüle ediyoruz
            _repositoryMock
                .Setup(r => r.GetByIdAsync(command.Id.Value, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingEntity);

            // 2. ACT
            var result = await _handler.Handle(command, CancellationToken.None);

            // 3. ASSERT
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);

            // Mapper'ın "request -> qrLocation" eşlemesini yapıp yapmadığını kontrol ediyoruz
            _mapperMock.Verify(m => m.Map(command, existingEntity), Times.Once);

            // Repository Update metodunun çağrıldığını doğruluyoruz
            _repositoryMock.Verify(r => r.Update(existingEntity), Times.Once);

            // Mapper'ın "qrLocation -> response" eşlemesini yapıp yapmadığını kontrol ediyoruz
            _mapperMock.Verify(m => m.Map(existingEntity, It.IsAny<UpdateQrLocationResponse>()), Times.Once);
        }
    }
}