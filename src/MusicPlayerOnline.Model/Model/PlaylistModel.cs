using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlayerOnline.Model.Model
{
    public class PlaylistModel : MusicInfoBase
    {
        public bool IsPlaying { get; set; }
        public string PlayUrl { get; set; }
    }
}
