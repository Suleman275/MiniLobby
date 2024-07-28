using MiniLobby.Enums;

namespace MiniLobby.Misc {
    public class DataPoint {
        public string Value { get; set; }
        public VisibilityOptions Visibility { get; set; }

        public DataPoint() {
            
        }

        public DataPoint(string value, VisibilityOptions visibility) {
            Value = value;
            Visibility = visibility;
        }
    }
}
