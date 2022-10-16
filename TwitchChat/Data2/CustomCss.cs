

namespace TwitchChat
{
    public class CustomCss
    {
        public string Title {get; set;}
        public string css { get; set; }


        public static string defaultCSS = @"::-webkit-scrollbar {


}

#chat_box {
    display: flex;
 /* flex-direction: column-reverse;
    position: sticky;
    top: 0;
*/
    flex-direction: column;
    justify-content: flex-end;
}

.chat_line
{
    line - height: 1;
    width: fit - content;
    color: white!important;
    margin: 20px;
    display: grid;
    grid - template - areas:
    'time nick colon'
    ' message message message';
    border - radius: 25px;
    background: black;
    border: 15px solid black;
    position: relative;
    -webkit - animation - delay: 0s, 20s;
    -webkit - animation - duration: 1s, 1s;
    -webkit - animation - name: enter, leave;
    -webkit - animation - fill - mode: forwards, forwards;
    -webkit - animation - timing - function: ease, ease;
}

@keyframes enter
{
    from
    {
        right: -100 %;
        opacity: 0;
        -webkit - transform: scale(0);
    }
    to
    {
        right: 0 %;
        opacity: 1;
        -webkit - transform: scale(1);
    }
}
@keyframes leave
{
    from
    {
        right: 0 %;
        opacity: 1;
        -webkit - transform: scale(1);
    }
    to
    {
        right: -100 %;
        opacity: 0;
        -webkit - transform: scale(0);
    }
}

.chat_line span
{
    display:none;
}

.chat_line.time_stamp {
    grid - area: time;
    display: none;
    padding - right: 3px;
}

.chat_line.nick {
    color: white!important;
    grid - area: nick;
    display: initial;
    font - size: 1.3rem;
    font - weight: 900;
}

.chat_line.colon{
    display: none;
    grid - area: colon;
}

.chat_line.message {
    grid - area: message;
    margin: 20px 0px 0px 0px;
    display: initial;
    word - wrap: break-word;
    font - size: 1.15rem;
}
//";
    }
}
