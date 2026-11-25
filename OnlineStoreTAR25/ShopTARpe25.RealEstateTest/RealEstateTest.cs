using ShopTARpe25.Core.Dto;
using ShopTARpe25.Core.ServiceInterface;

namespace ShopTARpe25.RealEstateTest
{
    public class RealEstateTest : TestBase
    {
        [Fact]
        public async Task ShouldNot_AddEmptyRealEstate_WhenReturnResult()
        {
            //Arrange
            RealEstateDto dto = new()
            {
                Area = 120.5,
                Address = "Test location",
                RoomNumber = 3,
                BuildingType = "house",
                CreatedAt = DateTime.UtcNow,
                ModifiedAt = DateTime.UtcNow,
            };

            //Tuleb kutsuda välja RealEstate Create meetod esile
            var result = await Svc<IRealEstateServices>().Create(dto);

            //Assert
            Assert.NotNull(result);

            //kuidas testi käima panna??
            //Run Tests - Test Explorer

        }
        //teha test, kus otsitakse ankeeti id alusel, aga seda ei ole andmebaasis
        [Fact]
        public async Task ShouldNot_GetByIdRealEstate_WhenReturnsEqual()
        {
            // Arrange
            Guid databaseGuid = Guid.Parse("d61e8ffa-9336-441b-af2d-4dcd15df0969");
            Guid wrongGuid = Guid.NewGuid();

            // Act - siia tuleb detailAsync
            await Svc<IRealEstateServices>().DetailAsync(databaseGuid);

            // Assert
            //vastupidist loogikat tuleb kasutada
            Assert.NotEqual(wrongGuid, databaseGuid);
        }

        [Fact]
        public async Task Should_GetByIdRealEstate_WhenReturnEqual()
        {
            //Arrange
            //see guid on andmebaasis juba olemas ankeedi juures
            Guid databaseGuid = Guid.Parse("d61e8ffa-9336-441b-af2d-4dcd15df0969");
            //kui vajutad nupule, siis see guid saadetakse andmebaasi, mida võrreldakse
            Guid guid = Guid.Parse("d61e8ffa-9336-441b-af2d-4dcd15df0969");

            //act
            await Svc<IRealEstateServices>().DetailAsync(guid);

            //Assert
            Assert.Equal(databaseGuid, guid);

        }

        [Fact]
        public async Task Should_DeleteByIdRealEstate_WhenDeleteRealEstate()
        {
            //Arrange
            RealEstateDto dto = MockRealEstateData();

            //Act
            var AddRealEstate = await Svc<IRealEstateServices>().Create(dto);
            var deleteRealEstate = await Svc<IRealEstateServices>().Delete((Guid)AddRealEstate.Id);

            //Assert
            //võrdlen Id-sid, kas kasutasin selle ära, mis oli andmebaasis
            Assert.Equal(AddRealEstate.Id, deleteRealEstate.Id);
        }

        private RealEstateDto MockRealEstateData()
        {
            return new RealEstateDto
            {
                Area = 88.65,
                Address = "TestAadress",
                RoomNumber = 1,
                BuildingType = "TestBuildingType",
                CreatedAt = DateTime.Now,
                ModifiedAt = DateTime.Now,
            };
        }
    }
}
