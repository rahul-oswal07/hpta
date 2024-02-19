namespace HPTA.Common
{
    public enum QuestionAnswerType
    {
        Rating, FreeText, SingleSelection, MultiSelection
    }

    public enum RatingValue
    {
        StronglyDisagree = 1, Disagree = 2, Neutral = 3, Agree = 4, StronglyAgree = 5
    }

    public enum Roles
    {
        Unknown = 0, TeamMember = 1, ScrumMaster = 2, QA = 3, Observer = 9, CDL = 10, MD = 20, Coach = 21, EngagementLead = 22
    }
}
