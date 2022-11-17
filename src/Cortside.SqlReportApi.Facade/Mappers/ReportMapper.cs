namespace Cortside.SqlReportApi.Facade.Mappers {

    public class ReportMapper {
        private readonly SubjectMapper subjectMapper;

        public ReportMapper(SubjectMapper subjectMapper) {
            this.subjectMapper = subjectMapper;
        }

        public CustomerDto MapToDto(Customer entity) {
            if (entity == null) {
                return null;
            }

            return new CustomerDto {
                CustomerId = entity.CustomerId,
                CustomerResourceId = entity.CustomerResourceId,
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                Email = entity.Email,
                CreatedDate = entity.CreatedDate,
                LastModifiedDate = entity.LastModifiedDate,
                CreatedSubject = subjectMapper.MapToDto(entity.CreatedSubject),
                LastModifiedSubject = subjectMapper.MapToDto(entity.LastModifiedSubject),
            };
        }
    }
}
