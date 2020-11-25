using System.Threading;
using CVService.Api.DataLayer;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace CVService.Api.Tests.Unit
{
    //TODO: Tech Test - I created some repository unit tests to prove how I would write and mock them but do not have full coverage as these are covered in integration tests.
    public class RepositoryBaseTestDouble
    {
        [Fact]
        public async void RepositoryBase_GetByIdAsync_IntPassed_ProperMethodCalled()
        {
            // Arrange
            var testId = 1;
            var contextMock = new Mock<ApiContext>(MockBehavior.Strict, new DbContextOptions<ApiContext>());

            contextMock.Setup(x => x.Set<HasIdTestDouble>().FindAsync(It.IsAny<int>())).Returns<HasIdTestDouble>(null);

            // Act
            var repository = new TestClassRepository(contextMock.Object);
            await repository.GetByIdAsync(testId);

            //Assert
            contextMock.Verify(x => x.Set<HasIdTestDouble>().FindAsync(testId));
        }

        [Fact]
        public async void RepositoryBase_AddAsync_TestClassObjectPassed_ProperMethodCalled()
        {
            // Arrange
            var testObject = new HasIdTestDouble();

            var contextMock = new Mock<ApiContext>(MockBehavior.Strict, new DbContextOptions<ApiContext>());
            var dbSetMock = new Mock<DbSet<HasIdTestDouble>>();
            contextMock.Setup(x => x.Set<HasIdTestDouble>()).Returns(dbSetMock.Object);

            contextMock.Setup(x => x.AddAsync(It.IsAny<HasIdTestDouble>(), It.IsAny<CancellationToken>()))
                .Returns<HasIdTestDouble>(null);

            contextMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            // Act
            var repository = new TestClassRepository(contextMock.Object);
            await repository.AddAsync(testObject);

            //Assert
            contextMock.Verify(x => x.Set<HasIdTestDouble>());
            dbSetMock.Verify(x => x.AddAsync(testObject, It.IsAny<CancellationToken>()));
            contextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()));
        }

        [Fact]
        public async void RepositoryBase_UpdateAsync_TestClassObjectPassed_ProperMethodCalled()
        {
            // Arrange
            var testObject = new HasIdTestDouble();

            var contextMock = new Mock<ApiContext>(MockBehavior.Strict, new DbContextOptions<ApiContext>());
            
            contextMock.Setup(x => x.SetModifiedState(testObject));
            contextMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            // Act
            var repository = new TestClassRepository(contextMock.Object);
            await repository.UpdateAsync(testObject);

            //Assert
            contextMock.Verify(x => x.SetModifiedState(testObject));
            contextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()));
        }

        [Fact]
        public async void RepositoryBase_RemoveAsync_TestClassObjectPassed_ProperMethodCalled()
        {
            // Arrange
            var testObject = new HasIdTestDouble();

            var contextMock = new Mock<ApiContext>(MockBehavior.Strict, new DbContextOptions<ApiContext>());
            var dbSetMock = new Mock<DbSet<HasIdTestDouble>>();
            contextMock.Setup(x => x.Set<HasIdTestDouble>()).Returns(dbSetMock.Object);

            contextMock.Setup(x => x.Remove(testObject))
                .Returns<HasIdTestDouble>(null);

            contextMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            // Act
            var repository = new TestClassRepository(contextMock.Object);
            await repository.RemoveAsync(testObject);

            //Assert
            contextMock.Verify(x => x.Set<HasIdTestDouble>());
            dbSetMock.Verify(x => x.Remove(testObject));
            contextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()));
        }
    }
}
