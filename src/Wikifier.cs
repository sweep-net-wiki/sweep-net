﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace papl
{
    class Wikifier
    {
        String text;
        List<String> hidden = new List<string>();
        char c1 = '\x01';
        char c2 = '\x02';

        void rrr(String a, String b)
        {
            text = text.Replace(a, b);
        }
        void rr(String r, String b)
        {
            Regex re = new Regex(r);
            text = re.Replace(text, b);
        }
        
        void hide(String re_s)
        {
            Regex re = new Regex(re_s);
            foreach (Match match in re.Matches(text))
            {
                rrr(match.Value, c1+(hidden.Count).ToString()+c2);
                hidden.Add(match.Value);
            }
        }
        void hideTag(String tag)
        {
            hide("/<" + tag + "( [^>]+)?>[\\s\\S]+?<\\/" + tag + ">/i");
        }


        public String Wikify(String tex)
        {
            text = tex;

            char u = '\u00A0';

            hideTag("nowiki");
            hideTag("pre");
            hideTag("source");
            hideTag("code");
            hideTag("tt");
            hideTag("math");



            rr("/( |\n|\r)+\\{\\{(·|•|\\*)\\}\\}/", "{{$2}}");

            hide("/{\\{[\\s\\S]+?}}/");
            hide("/^ .*/m");
            hide("/(https?|ftp|news|nntp|telnet|irc|gopher):\\/\\/[^\\s\\[\\]<>\"]+ ?/i");
            hide("/^#(redirect|перенапр(авление)?)/i");
            hideTag("gallery");


            rr("/ +(\n|\r)/", "$1");
            text = '\n' + text + '\n';

            //LINKS
           // rr("/(\\[\\[:?)(category|категория):( *)/i", "$1Категория:");
          //  rr("/(\\[\\[:?)(image|изображение|file):( *)/i", "$1Файл:");
            //Linked years, centuries and ranges
            rr("/(\\(|\\s)(\\[\\[[12]?\\d{3}\\]\\])[\\u00A0 ]?(-{1,3}|–|—) ?(\\[\\[[12]?\\d{3}\\]\\])(\\W)/", "$1$2—$4$5");
           // rr("/(\\[\\[[12]?\\d{3}\\]\\]) ?(гг?\\.)/", "$1" + u + "$2");
            rr("/(\\(|\\s)(\\[\\[[IVX]{1,5}\\]\\])[\\u00A0 ]?(-{1,3}|–|—) ?(\\[\\[[IVX]{1,5}\\]\\])(\\W)/", "$1$2—$4$5");
           // rr("/(\\[\\[[IVX]{1,5}\\]\\]) ?(вв?\\.)/", "$1" + u + "$2");
            //rr("/\\[\\[(\\d+)\\]\\]\\sгод/", "[[$1" + u + "год]]");
           // rr("/\\[\\[(\\d+)\\sгод\\|\\1\\]\\]\\sгод/", "[[$1" + u + "год]]");
           // rr("/\\[\\[(\\d+)\\sгод\\|\\1\\sгод([а-я]{0,3})\\]\\]/", "[[$1" + u + "год]]$2");
           // rr("/\\[\\[((\\d+)(?: (?:год )?в [\\wa-яёА-ЯЁ ]+\\|\\2)?)\\]\\][\\u00A0 ](год[а-яё]*)/", "[[$1" + u + "$3]]");
           // rr("/\\[\\[([XVI]+)\\]\\]\\sвек/", "[[$1" + u + "век]]");
          //  rr("/\\[\\[([XVI]+)\\sвек\\|\\1\\]\\]\\sвек/", "[[$1" + u + "век]]");
          //  rr("/\\[\\[([XVI]+)\\sвек\\|\\1\\sвек([а-я]{0,3})\\]\\]/", "[[$1" + u + "век]]$2");
          //  rr("/\\[\\[(([XVI]+) век\\|\\2)\\]\\][\\u00A0 ]век/", "[[$2" + u + "век]]");
            // Nice links
            rr("/(\\[\\[[^|\\[\\]]*)[\\u00AD\\u200E\\u200F]+([^\\[\\]]*\\]\\])/", "$1$2");
         //   rr("/\\[\\[ *([a-zA-Zа-яёА-ЯЁ\\u00A0-\\u00FF %!\\\"$&'()*,\\-—.\\/0-9:;=?\\\\@\\^_`’~]+) *\\| *(\\1)([a-zа-яё]*) *\\]\\]/", "[[$2]]$3");
            rr("/\\[\\[ *([^|\\[\\]]+)([^|\\[\\]]+) *\\| *\\1 *\\]\\]\\2/", "[[$1$2]]");
            rr("/\\[\\[ *(?!Файл:|Категория:)([a-zA-Zа-яёА-ЯЁ\\u00A0-\\u00FF %!\\\"$&'()*,\\-—.\\/0-9:;=?\\\\@\\^_`’~]+) *\\| *([^|[\\]]+) *\\]\\]([a-zа-яё]+)/", "[[$1|$2$3]]");

            hide("/\\[\\[[^\\]|]+/");

            //TAGS

            rr("/(sup>|sub>|\\s)-(\\d)/", "$1?$2"); //minus
            rr("/&sup2;/i", "?");
            rr("/&sup3;/i", "?");
            rr("/<(b|strong);>(.*?);<\\/(b|strong);>/i", "'''$2'''");
            rr("/<(i|em);>(.*?);<\\/(i|em);>/i", "''$2''");
            rr("/^<hr ?\\/?>/im", "----");
            rr("/<\\/?(hr|br)( [^\\/>]+?)? ?\\/?>/i", "<$1$2 />");
         //   rr("/(\\n== *[a-zа-я\\s\\.:]+ *==\\n+)<references *\\/>/i","$1{\\{примечания}}");

            hide("/^({\\||\\|-).*/m");
            hide("/(^\\||^!|!!|\\|\\|) *[a-z]+=[^|]+\\|(?!\\|)/mi");//cell style
            hide("/\\| +/g");//formatted cell

            rr("/[ \\t]+/"," ");

            rr("/^(=+)[ \\t\\f\\v]*(.*?)[ \\t\\f\\v]*=+$/m", "$1 $2 $1"); //add spaces inside
            rr("/([^\\r\\n])(\\r?\\n==.*==\\r?\\n)/", "$1\\n$2"); //add empty line before

            rr("/^== (.+)[.:] ==$/m", "== $1 ==");
            rr("/«|»|“|”|„/", "\"");//temp
            rr("/–/", "-"); //&ndash; -> hyphen
            rr("/&(#151|[nm]dash);/", "—"); // -> &mdash;
            rr("/(&nbsp;|\\s)-{1,3} /", "$1— "); // hyphen -> &mdash;
            rr("/(\\d)--(\\d)/", "$1—$2"); // -> &mdash;

            rr("/&copy;/i", "©");
            rr("/&reg;/i", "®");
            rr("/&sect;/i", "§");
            rr("/&euro;/i", "€");
            rr("/&yen;/i", "?");
            rr("/&pound;/i", "?");
            rr("/&deg;/", "°");
            rr("/\\(tm\\)|&trade;/i", "™");
            rr("/\\.\\.\\.|&hellip;/", "…");
            rr("/\\+-(?!\\+|-)|&plusmn;/", "±");
            rr("/~=/", "?");
            rr("/\\^2(\\D)/", "?$1");
            rr("/\\^3(\\D)/", "?$1");
            rr("/&((la|ra|bd|ld)quo|quot);/", "\"");
            rr("/([\\wа-яА-ЯёЁ])'([\\wа-яА-ЯёЁ])/", "$1’$2"); //'
            rr("/№№/", "№");

            rr("/(\\(|\\s)([12]?\\d{3})[\\u00A0 ]?(-{1,3}|—) ?([12]?\\d{3})(?![\\w-])/", "$1$2—$4");
            rr("/([12]?\\d{3}) ?(гг?\\.)/", "$1" + u + "$2");
            rr("/(\\(|\\s)([IVX]{1,5})[\\u00A0 ]?(-{1,3}|—) ?([IVX]{1,5})(?![\\w-])/", "$1$2—$4");
            rr("/([IVX]{1,5}) ?(вв?\\.)/", "$1" + u + "$2");

            rr("/ISBN:\\s?(?=[\\d\\-]{8,17})/", "ISBN ");

            rr("/^([#*:]+)[ \\t\\f\\v]*(?!\\{\\|)([^ \\t\\f\\v*#:;])/m", "$1 $2"); //space after #*: unless before table
            rr("/(\\S) (-{1,3}|—) (\\S)/", "$1" + u + "— $3");
            rr("/([А-Я]\\.) ?([А-Я]\\.) ?([А-Я][а-я])/", "$1" + u + "$2" + u + "$3");
            rr("/([А-Я]\\.)([А-Я]\\.)/", "$1 $2");
            rr("/([а-я]\\.)([А-ЯA-Z])/", "$1 $2"); // word. word
            rr("/([)\"а-яa-z\\]])\\s*,([\\[(\"а-яa-z])/", "$1, $2"); // word, word
            rr("/([)\"а-яa-z\\]])\\s([,;])\\s([\\[(\"а-яa-z])/", "$1$2 $3");
            rr("/([^%\\/\\w]\\d+?(?:[.,]\\d+?)?) ?([%‰])(?!-[А-Яа-яЁё])/", "$1" + u + "$2"); //5 %
            rr("/(\\d) ([%‰])(?=-[А-Яа-яЁё])/", "$1$2"); //5%-й
            rr("/([№§])(\\s*)(\\d)/", "$1" + u + "$3");
            rr("/\\( +/", "("); ;
            rr("/ +\\)/", ")"); //inside ()

            rr("/([\\s\\d=????<>—(\"'|])([+±?-]?\\d+?(?:[.,]\\d+?)?)(([ °^*]| [°^*])C)(?=[\\s\"').,;!?|])/m", "$1$2"+u+"°C");
            rr("/([\\s\\d=????<>—(\"'|])([+±?-]?\\d+?(?:[.,]\\d+?)?)(([ °^*]| [°^*])F)(?=[\\s\"').,;|!?])/m", "$1$2"+u+"°F"); //'

            rr("/(\\s\\d+)\\.(\\d+[\\u00A0 ]*[%‰°])/i", "$1,$2");

            return text;
        }
    }
}
