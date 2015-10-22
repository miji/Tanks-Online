[System.AttributeUsage(System.AttributeTargets.Class |
                       System.AttributeTargets.Struct)
]
public class MHelpDesc : System.Attribute
{
    private string description;

    public string Description
    {
        get { return description; }
    }

    public MHelpDesc(string description)
    {
        this.description = description;
    }
}