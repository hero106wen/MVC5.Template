﻿using Moq;
using NUnit.Framework;
using Template.Components.Services;
using Template.Data.Core;

namespace Template.Tests.Unit.Components.Services
{
    [TestFixture]
    public class BaseServiceTests
    {
        private Mock<IUnitOfWork> unitOfWorkMock;
        private BaseService service;

        [SetUp]
        public void SetUp()
        {
            unitOfWorkMock = new Mock<IUnitOfWork>();
            service = new Mock<BaseService>(unitOfWorkMock.Object) { CallBase = true }.Object;
        }

        [TearDown]
        public void TearDown()
        {
            service.Dispose();
        }

        #region Method: Dispose()

        [Test]
        public void Dispose_CallsUnitOfWorkDispose()
        {
            service.Dispose();
            
            unitOfWorkMock.Verify(mock => mock.Dispose(), Times.Once());
        }

        [Test]
        public void Dispose_CanDisposeMultipleTimes()
        {
            service.Dispose();
            service.Dispose();
        }

        #endregion
    }
}
