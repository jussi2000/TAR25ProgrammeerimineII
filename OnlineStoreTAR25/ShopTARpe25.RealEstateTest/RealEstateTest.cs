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

        //mul on kaks ankeeti loodud ja ühe kustutan ära 
        //assertis võrdlen, kas need kaks on ühesugused
        [Fact]
        public async Task ShouldNot_DeleteByIdRealEstate_WhenDidNotDeleteRealEstate()
        {
            //Arrange
            RealEstateDto dto = MockRealEstateData();

            var addRealEstate1 = await Svc<IRealEstateServices>().Create(dto);
            var addRealEstate2 = await Svc<IRealEstateServices>().Create(dto);

            //Act
            var result = await Svc<IRealEstateServices>().Delete((Guid)addRealEstate2.Id);

            //Assert
            Assert.NotEqual(addRealEstate1.Id, result.Id);
        }

        //uuendab andmeid
        [Fact]
        public async Task Should_UpdateRealEstate_WhenUpdateData()
        {
            //Arrange
            var guid = new Guid("d61e8ffa-9336-441b-af2d-4dcd15df0969");

            RealEstateDto dto = MockRealEstateData();

            RealEstateDto domain = new();
            domain.Id = guid;
            domain.Area = 200.0;
            domain.Address = "UpdateLocation";
            domain.BuildingType = "Villa";
            domain.CreatedAt = DateTime.UtcNow;
            domain.ModifiedAt = DateTime.UtcNow;

            //Act
            await Svc<IRealEstateServices>().Update(dto);

            //Assert, 5tk teha

            Assert.Equal(domain.Id, guid);
            Assert.NotEqual(dto.Area, domain.Area);
            Assert.NotEqual(dto.RoomNumber, domain.RoomNumber);
            //võrrelda RoomNumber'it ja kasutada DoesNotMatch
            //DoesNotMatch tahab string andmetüüpe
            Assert.DoesNotMatch(dto.RoomNumber.ToString(), domain.RoomNumber.ToString());
            Assert.DoesNotMatch(dto.Address, domain.Address);

        }

        //[Fact]
        public async Task Should_UpdateRealEstate_WhenUpdateDataVersion2()
        {

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
