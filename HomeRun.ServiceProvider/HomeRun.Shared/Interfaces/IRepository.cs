using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

namespace HomeRun.Shared
{


    public class BaseEntity
    {
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int      Id        { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow; 
    }

    public interface IRepository<EntityType> where EntityType : class
    {
        Task    <IEnumerable<EntityType>>   GetAll                                              ();
        Task    <EntityType?>               Get             (Guid id                             );
        Task    <EntityType?>               Get             (EntityType? entity                  );

        Task    <EntityType?>               Create          (EntityType? entity                  );
        Task    <IEnumerable<EntityType>>   Where(Expression<Func<EntityType, bool>> predicate   );
        Task    <IEnumerable<EntityType>?>  CreateMany      (IEnumerable<EntityType> entities    );
        Task    <EntityType?>               Update          (Guid id, EntityType? entity         );
        Task                                Delete          (Guid id                             );
        Task                                DeleteMany      (IEnumerable<EntityType> entities    );


    }
}
