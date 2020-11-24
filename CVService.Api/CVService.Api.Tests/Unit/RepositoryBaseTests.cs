using System.Threading;
using CVService.Api.DataLayer;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace CVService.Api.Tests.Unit
{
    //TODO: Tech Test - I created some repository unit tests to prove how I would write and mock them but do not have full coverage as these are covered in integration tests.
    public class RepositoryBaseTests
    {
        [Fact]
        public async void RepositoryBase_GetByIdAsync_IntPassed_ProperMethodCalled()
        {
            // Arrange
            var testId = 1;
            var contextMock = new Mock<ApiContext>(MockBehavior.Strict, new DbContextOptions<ApiContext>());

            contextMock.Setup(x => x.Set<TestClass>().FindAsync(It.IsAny<int>())).Returns<TestClass>(null);

            // Act
            var repository = new TestClassRepository(contextMock.Object);
            await repository.GetByIdAsync(testId);

            //Assert
            contextMock.Verify(x => x.Set<TestClass>().FindAsync(testId));
        }

        [Fact]
        public async void RepositoryBase_AddAsync_TestClassObjectPassed_ProperMethodCalled()
        {
            // Arrange
            var testObject = new TestClass();

            var contextMock = new Mock<ApiContext>(MockBehavior.Strict, new DbContextOptions<ApiContext>());
            var dbSetMock = new Mock<DbSet<TestClass>>();
            contextMock.Setup(x => x.Set<TestClass>()).Returns(dbSetMock.Object);

            contextMock.Setup(x => x.AddAsync(It.IsAny<TestClass>(), It.IsAny<CancellationToken>()))
                .Returns<TestClass>(null);

            contextMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            // Act
            var repository = new TestClassRepository(contextMock.Object);
            await repository.AddAsync(testObject);

            //Assert
            contextMock.Verify(x => x.Set<TestClass>());
            dbSetMock.Verify(x => x.AddAsync(testObject, It.IsAny<CancellationToken>()));
            contextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()));
        }

        [Fact]
        public async void RepositoryBase_UpdateAsync_TestClassObjectPassed_ProperMethodCalled()
        {
            // Arrange
            var testObject = new TestClass();

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
            var testObject = new TestClass();

            var contextMock = new Mock<ApiContext>(MockBehavior.Strict, new DbContextOptions<ApiContext>());
            var dbSetMock = new Mock<DbSet<TestClass>>();
            contextMock.Setup(x => x.Set<TestClass>()).Returns(dbSetMock.Object);

            contextMock.Setup(x => x.Remove(testObject))
                .Returns<TestClass>(null);

            contextMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            // Act
            var repository = new TestClassRepository(contextMock.Object);
            await repository.RemoveAsync(testObject);

            //Assert
            contextMock.Verify(x => x.Set<TestClass>());
            dbSetMock.Verify(x => x.Remove(testObject));
            contextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()));
        }
    }
}
