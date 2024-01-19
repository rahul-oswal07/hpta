namespace HPTA.Common.Constraints;

public interface IAuditable
{
    DateTime CreatedOn { get; set; }

    DateTime UpdatedOn { get; set; }
}
