using System.Collections.Generic;

namespace TF_IDF_Maker.Model
{
    public class TFIDFNote
    {
        /// <summary>
        /// Text
        /// </summary>
        public string Word { get; set; }

        /// <summary>
        /// TF-IDF value
        /// </summary>
        public List<TFIDFValue> ValuesList { get; set; }
    }
}
