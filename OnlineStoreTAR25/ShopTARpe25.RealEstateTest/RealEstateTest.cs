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
            //tuleb kasutada MockRealEstateDataUpdate ja MockRealEstateData2
            RealEstateDto dto = MockRealEstateData2();
            RealEstateDto Updatedto = MockRealEstateDataUpdate();

            //Act
            var createRealEstate = await Svc<IRealEstateServices>().Create(dto);
            var result = await Svc<IRealEstateServices>().Update(dto);

            //Assert
            Assert.DoesNotMatch(createRealEstate.Address, result.Address);
            Assert.NotEqual(createRealEstate.ModifiedAt, result.ModifiedAt);
        }






        //kasutame ankeeti kus kõik andmed on null ja
        //tuleb kasutada create'i ja update'i
        //iseseisev töö
        //ei tööta hetkel
        [Fact]
        public async Task Should_CreateUpdateDeleteRealEstate()
        {
            var guid = Guid.NewGuid();

            //Arrange
            RealEstateDto dto = new()
            {
                Id = guid,
                Area = 100,
                Address = "Test location1",
                RoomNumber = 1,
                BuildingType = "house1",
                CreatedAt = DateTime.UtcNow,
                ModifiedAt = DateTime.UtcNow,
            };

            RealEstateDto dto2 = new();
            
            dto2.Area = 200;
            dto2.Address = "Test location2";
            dto2.RoomNumber = 2;
            dto2.BuildingType = "house2";
            dto2.CreatedAt = DateTime.UtcNow;
            dto2.ModifiedAt = DateTime.UtcNow;

            //Act
            var addRealEstate = await Svc<IRealEstateServices>().Create(dto);
            var UpdateRealEstate = await Svc<IRealEstateServices>().Update(dto2);
            var result = await Svc<IRealEstateServices>().Delete((Guid)UpdateRealEstate.Id);

            //Assert
            Assert.NotEqual(addRealEstate, UpdateRealEstate);
            //Assert.Null(result);
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

        private RealEstateDto MockRealEstateData2()
        {
            return new RealEstateDto
            {
                Area = 120.00,
                Address = "Sample Address",
                RoomNumber = 8,
                BuildingType = "House",
                CreatedAt = DateTime.Now,
                ModifiedAt = DateTime.Now,
            };
        }

        private RealEstateDto MockRealEstateDataUpdate()
        {
            return new RealEstateDto
            {
                Area = 100.00,
                Address = "Woodland",
                RoomNumber = 2,
                BuildingType = "Hideout",
                CreatedAt = DateTime.Now,
                ModifiedAt = DateTime.Now,
            };
        }

    }
}
