using ModernWpf.Controls.Primitives;

namespace TranscribeMe.Model {
    public class FormData {
        public string Placeholder { get; set; }
        public TextBox TextBox { get; set; }

        public FormData() {

            TextBox textBox = new();
            ControlHelper.SetCornerRadius(textBox, new CornerRadius(5));
            ControlHelper.SetPlaceholderText(textBox, Placeholder);


        }

    }
}
