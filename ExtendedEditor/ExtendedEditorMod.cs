using ICities;
using UnityEngine;

namespace ExtendedEditor
{
    public class ExtendedEditorMod : IUserMod
    {
        public string Description
        {
            get { return "Extends the asset editor to support editing building attributes that are hidden in the default editor"; }
        }

        public string Name
        {
            get { return "Extended Asset Editor"; }
        }
    }
}
