namespace ShopTARpe25.Core.Dto
{
    public class FileToApiDto
    {
        public Guid Id { get; set; }

        //see muutuja hakkab näitama, kus asub meie file
        //ning see on ühenduses kosmoselaevaga
        public string ExistingFilePath { get; set; }
        public Guid? SpaceshipId { get; set; }
    }
}