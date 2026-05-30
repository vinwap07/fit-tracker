using Domain.ValueObjects;

namespace Domain.Aggregates;

public class PersonalHealthAccount: IAggregateRoot
{
    public Guid Id { get; private set; }
    public UserProfile Profile { get; private set; }
    
    protected PersonalHealthAccount() {}
    public PersonalHealthAccount(Guid id, Height height, Weight weight, DateOnly dateOfBirth, Sex sex)
    {
        Id = id;
        Profile = new UserProfile(height, weight, dateOfBirth, sex);
    }

    public void UpdateProfile(Height? height, Weight? weight, DateOnly? dateOfBirth, Sex? sex)
    {
        Profile = new UserProfile(
            height ?? Profile.Height,
            weight ?? Profile.Weight,
            dateOfBirth ?? Profile.DateOfBirth,
            sex ?? Profile.Sex);
    }
}