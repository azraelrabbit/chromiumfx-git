//example for usage:

//monaco.editor.create(document.getElementById("container"), {
//	value: getCode(),
//	language: 'lua'
//});

function registerLua() {
    monaco.languages.register({ id: 'lua' });

    monaco.languages.registerCompletionItemProvider('lua', {
        provideCompletionItems: () => {
            return [
                {
                    "label": "if",
                    "Kind": 14,
                    "documentation": "if",
                    "insertText": "if {{condition}} then\n\t{{}}\nend"
                },
                {
                    "label": "ifel",
                    "Kind": 14,
                    "documentation": "ifel",
                    "insertText": "if {{condition}} then\n\t{{}}\nelse\n\t{{}}\nend"
                },
                {
                    "label": "elif",
                    "Kind": 14,
                    "documentation": "elif",
                    "insertText": "else if {{condition}} then\n\t{{}}\n"
                },
                {
                    "label": "for",
                    "Kind": 14,
                    "documentation": "for i=1,10",
                    "insertText": "for {{i}}={{1}},{{10}} do\n\t{{  }}\nend"
                },
                {
                    "label": "forp",
                    "Kind": 14,
                    "documentation": "for k,v in pairs()",
                    "insertText": "for {{k}},{{v}} in pairs({{tablename}) do\n\t{{print(k,v);}}\nend"
                },
                {
                    "label": "fun",
                    "Kind": 14,
                    "documentation": "function",
                    "insertText": "function {{function_name}}( {{params}} )\n\t{{\t}}\nend"
                },
                {
                    "label": "function",
                    "Kind": 14,
                    "documentation": "function",
                    "insertText": "function {{function_name}}( {{params}} )\n\t{{\t}}\nend"
                },
                {
                    "label": "local",
                    "Kind": 14,
                    "documentation": "local varname = ",
                    "insertText": "local {{varname}} = {{}}"
                },
                {
                    "label": "return",
                    "Kind": 14,
                    "documentation": "return ...",
                    "insertText": "return {{}}"
                },
                {
                    "label": "req",
                    "Kind": 1,
                    "documentation": "require('module name')",
                    "insertText": "require('{{module}}')"
                },
                {
                    "label": "require",
                    "Kind": 1,
                    "documentation": "require('module name')",
                    "insertText": "require('{{module}}')"
                },
                {
                    "label": "ver",
                    "Kind": 13,
                    "documentation": "_VERSION",
                    "insertText": "_VERSION"
                },
                {
                    "label": "version",
                    "Kind": 13,
                    "documentation": "_VERSION",
                    "insertText": "_VERSION"
                },
                {
                    "label": "assert",
                    "Kind": 1,
                    "documentation": "assert([v],[,message])",
                    "insertText": "assert({{v}}{})"
                },
                {
                    "label": "collectgarbage",
                    "Kind": 1,
                    "documentation": "collectgarbage([opt],[,arg])",
                    "insertText": "collectgarbage({{opt}},{{arg}})"
                },
                {
                    "label": "dofile",
                    "Kind": 1,
                    "documentation": "dofile ([filename])",
                    "insertText": "dofile({{filename}})"
                },
                {
                    "label": "getmetatable",
                    "Kind": 1,
                    "documentation": "getmetatable (object)",
                    "insertText": "getmetatable({{object}})"
                },
                {
                    "label": "next",
                    "Kind": 1,
                    "documentation": "next (table [, index])",
                    "insertText": "next({{tablename}},{{index}})"
                },
                {
                    "label": "print",
                    "Kind": 1,
                    "documentation": "print(...)",
                    "insertText": "print({{}})"
                },
                {
                    "label": "select",
                    "Kind": 1,
                    "documentation": "select (index, ···)",
                    "insertText": "select({{index}}, {{}})"
                },
                {
                    "label": "setmetatable",
                    "Kind": 1,
                    "documentation": "setmetatable (table, metatable)",
                    "insertText": "setmetatable({{tablename}}, {{metatable}})"
                },
                {
                    "label": "tonumber",
                    "Kind": 1,
                    "documentation": "tonumber (e [, base])",
                    "insertText": "tonumber({{e}})"
                },
                {
                    "label": "tostring",
                    "Kind": 1,
                    "documentation": "tostring (v)",
                    "insertText": "tostring({{}})"
                },
                {
                    "label": "type",
                    "Kind": 1,
                    "documentation": "type (v)",
                    "insertText": "type({{}})"
                },
                {
                    "label": "table",
                    "Kind": 13,
                    "documentation": "table",
                    "insertText": "table"
                },
                {
                    "label": "table.concat",
                    "Kind": 1,
                    "documentation": "table.concat (table [, sep [, start [, end]]]): concat是concatenate(连锁, 连接)的缩写. table.concat()函数列出参数中指定table的数组部分从start位置到end位置的所有元素, 元素间以指定的分隔符(sep)隔开。",
                    "insertText": "table.concat( {{tablename}},'{{,}}'},{start_index},{{end_index}})"
                },
                {
                    "label": "table.insert",
                    "Kind": 1,
                    "documentation": "table.insert (table, [pos,] value):在table的数组部分指定位置(pos)插入值为value的一个元素. pos参数可选, 默认为数组部分末尾.",
                    "insertText": "table.insert( {{tablename}},{{pos}},{{value}} )"
                },
                {
                    "label": "table.remove",
                    "Kind": 1,
                    "documentation": "table.remove (table [, pos]).返回table数组部分位于pos位置的元素. 其后的元素会被前移. pos参数可选, 默认为table长度, 即从最后一个元素删起.",
                    "insertText": "table.remove( {{tablename}},{{pos}} )"
                },
                {
                    "label": "table.sort",
                    "Kind": 1,
                    "documentation": "\t table.sort (table [, comp]).对给定的table进行升序排序,comp指的是排序委托方法",
                    "insertText": "table.sort( {{tablename}},{{sort_function}} )"
                },
                {
                    "label": "string",
                    "Kind": 13,
                    "documentation": "string object",
                    "insertText": "string"
                },
                {
                    "label": "string.byte",
                    "Kind": 1,
                    "documentation": "string.byte(arg[,int]),转换字符为整数值(可以指定某个字符，默认第一个字符)",
                    "insertText": "string.byte({{string}},{{0}})"
                },
                {
                    "label": "string.char",
                    "Kind": 1,
                    "documentation": "string.char(args),char 将整型数字转成字符并连接",
                    "insertText": "string.char({{}})"
                },
                {
                    "label": "string.find",
                    "Kind": 1,
                    "documentation": "string.strfind (str, substr, [init, [end]]),在一个指定的目标字符串中搜索指定的内容(第三个参数为索引),返回其具体位置。不存在则返回 nil",
                    "insertText": "string.find( {{string}},{{substr}})"
                },
                {
                    "label": "string.format",
                    "Kind": 1,
                    "documentation": "string.format(stringformat,args),string.format('the value is:%d',4),返回一个类似printf的格式化字符串",
                    "insertText": "string.format({{stringformat}},{{}})"
                },
                {
                    "label": "string.gmatch",
                    "Kind": 1,
                    "documentation": "string.gmatch (s, pattern),返回一个迭代器函数。 每次调用这个函数都会继续以 pattern 对 s 做匹配，并返回所有捕获到的值。 如果 pattern 中没有指定捕获，则每次捕获整个 pattern",
                    "insertText": "string.gmatch( {{string}},{{pattern}} )"
                },
                {
                    "label": "string.gsub",
                    "Kind": 1,
                    "documentation": "string.gsub(mainString,findString,replaceString,replaceCount),在字符串中替换,mainString为要替换的字符串,并返回其副本， findString 为被替换的字符，replaceString 要替换的字符，replaceCount 替换次数（可以忽略，则全部替换），如:string.gsub(\"aaaa\",\"a\",\"z\",3);\"",
                    "insertText": "string.gsub( {{mainstring}},{{findstring}},{{replacestring}})"
                },
                {
                    "label": "string.sub",
                    "Kind": 1,
                    "documentation": "string.sub (s, i [, j]),返回 s 的子串， 该子串从 i 开始到 j 为止； i 和 j 都可以为负数。 如果不给出 j ，就当它是 -1 （和字符串长度相同）。 特别是， 调用 string.sub(s,1,j) 可以返回 s 的长度为 j 的前缀串， 而 string.sub(s, -i) 返回长度为 i 的后缀串",
                    "insertText": "string.sub( {{string}}, {{i}} )"
                },
                {
                    "label": "string.len",
                    "Kind": 1,
                    "documentation": "string.len (s),接收一个字符串，返回其长度。 空串 '' 的长度为 0 。 内嵌零也统计在内，因此 'a\\000bc\\000' 的长度为 5",
                    "insertText": "string.len({{string}})"
                },
                {
                    "label": "string.lower",
                    "Kind": 1,
                    "documentation": "string.lower (s),接收一个字符串，将其中的大写字符都转为小写后返回其副本。 其它的字符串不会更改。 对大写字符的定义取决于当前的区域设置",
                    "insertText": "string.lower( {{string}} )"
                },
                {
                    "label": "string.match",
                    "Kind": 1,
                    "documentation": "string.match (s, pattern [, init]),在字符串 s 中找到第一个能用 pattern匹配到的部分。 如果能找到，match 返回其中的捕获物； 否则返回 nil 。 如果 pattern 中未指定捕获， 返回整个 pattern 捕获到的串。 第三个可选数字参数 init 指明从哪里开始搜索； 它默认为 1 且可以是负数",
                    "insertText": "string.match( {{string}},{{pattern}})"
                },
                {
                    "label": "string.upper",
                    "Kind": 1,
                    "documentation": "string.upper (s),接收一个字符串，将其中的小写字符都转为大写后返回其副本。 其它的字符串不会更改。 对小写字符的定义取决于当前的区域设置",
                    "insertText": "string.upper( {{string}} )"
                },
                {
                    "label": "string.rep",
                    "Kind": 1,
                    "documentation": "string.rep (string, num [, sep]),返回 num 个字符串 string 以字符串 sep 为分割符连在一起的字符串。 默认的 sep 值为空字符串（即没有分割符）。 如果 num 不是正数则返回空串",
                    "insertText": "string.rep({{string}}, {{num}} )"
                },
                {
                    "label": "string.reverse",
                    "Kind": 1,
                    "documentation": "string.reverse (s),返回字符串 s 的翻转串",
                    "insertText": "string.reverse( {{}})"
                },
                {
                    "label": "string.pack",
                    "Kind": 1,
                    "documentation": "string.pack (fmt, v1, v2 , ...),返回一个打包了（即以二进制形式序列化） v1, v2 等值的二进制字符串。 字符串 fmt 为打包格式",
                    "insertText": "string.pack()"
                },
                {
                    "label": "string.unpack",
                    "Kind": 1,
                    "documentation": "string.unpack (fmt, s [, pos]),返回以格式 fmt打包在字符串 s （参见 string.pack）中的值。 选项 pos（默认为 1 ）标记了从 s 中哪里开始读起。 读完所有的值后，函数返回 s 中第一个未读字节的位置",
                    "insertText": "string.unpack( {{stringformat},{{string}}})"
                },
                {
                    "label": "string.packsize",
                    "Kind": 1,
                    "documentation": "string.packsize (stringformat),返回以指定格式用 string.pack 打包的字符串的长度。 格式化字符串中不可以有变长选项 's' 或 'z' ",
                    "insertText": "string.packsize()"
                },
                {
                    "label": "string.dump",
                    "Kind": 1,
                    "documentation": "string.dump (function [, strip]),返回包含有以二进制方式表示的（一个 二进制代码块 ）指定函数的字符串。 之后可以用 load 调用这个字符串获得 该函数的副本（但是绑定新的上值）。 如果　strip 为真值， 二进制代码块不携带该函数的调试信息 （局部变量名，行号，等等。）",
                    "insertText": "string.dump( {{function}})"
                },
                {
                    "label": "math",
                    "Kind": 13,
                    "documentation": "math",
                    "insertText": ""
                },
                {
                    "label": "math.abs",
                    "Kind": 1,
                    "documentation": "math.abs (x),返回 x 的绝对值。(integer/float)",
                    "insertText": "math.abs({{x}})"
                },
                {
                    "label": "math.acos",
                    "Kind": 1,
                    "documentation": "math.acos(x),返回 x 的反余弦值（用弧度表示）",
                    "insertText": "math.acos( {{x}} )"
                },
                {
                    "label": "math.asin",
                    "Kind": 1,
                    "documentation": "math.asin (x),返回 x 的反正弦值（用弧度表示）",
                    "insertText": "math.asin({{x}} )"
                },
                {
                    "label": "math.atan",
                    "Kind": 1,
                    "documentation": "math.atan (y [, x]),返回 y/x 的反正切值（用弧度表示）。 它会使用两个参数的符号来找到结果落在哪个象限中,默认的 x 是 1 ， 因此调用 math.atan(y) 将返回 y 的反正切值",
                    "insertText": "math.atan( {{y}} )"
                },
                {
                    "label": "math.ceil",
                    "Kind": 1,
                    "documentation": "math.ceil (x),返回不小于 x 的最小整数值",
                    "insertText": "ceil({{x}} )"
                },
                {
                    "label": "math.cos",
                    "Kind": 1,
                    "documentation": "math.cos (x),返回 x 的余弦（假定参数是弧度）",
                    "insertText": "math.cos({{x}})"
                },
                {
                    "label": "math.deg",
                    "Kind": 1,
                    "documentation": "math.deg(x),将角 x 从弧度转换为角度",
                    "insertText": "math.deg({{x}})"
                },
                {
                    "label": "math.exp",
                    "Kind": 1,
                    "documentation": "math.exp (x),返回 ex 的值 （e 为自然对数的底）",
                    "insertText": "math.exp({{x}} )"
                },
                {
                    "label": "math.pi",
                    "Kind": 13,
                    "documentation": "π 的值",
                    "insertText": "math.pi"
                },
                {
                    "label": "math.rad",
                    "Kind": 1,
                    "documentation": "math.rad (x),将角 x 从角度转换为弧度",
                    "insertText": "math.rad({{x}})"
                },
                {
                    "label": "math.floor",
                    "Kind": 1,
                    "documentation": "math.floor (x),返回不大于 x 的最大整数值",
                    "insertText": "math.floor({{x}})"
                },
                {
                    "label": "math.modf",
                    "Kind": 1,
                    "documentation": "math.modf (x),返回 x 的整数部分和小数部分。 第二个结果一定是浮点数",
                    "insertText": "math.modf( {{x}} )"
                },
                {
                    "label": "math.random",
                    "Kind": 1,
                    "documentation": "math.random ([m [, n]]),当不带参数调用时， 返回一个 [0,1) 区间内一致分布的浮点伪随机数。 当以两个整数 m 与 n 调用时， math.random 返回一个 [m, n] 区间 内一致分布的整数伪随机数。 （值 m-n 不能是负数，且必须在 Lua 整数的表示范围内。） 调用 math.random(n) 等价于 math.random(1,n)",
                    "insertText": "math.random( {{m}},{{n}})"
                },
                {
                    "label": "math.randomseed",
                    "Kind": 1,
                    "documentation": "math.randomseed (x),把 x 设为伪随机数发生器的 种子, 相同的种子产生相同的随机数列",
                    "insertText": "math.randomseed( {{x}} )"
                },
                {
                    "label": "math.sin",
                    "Kind": 1,
                    "documentation": "math.sin (x),返回 x 的正弦值（假定参数是弧度）",
                    "insertText": "math.sin( {{x}} )"
                },
                {
                    "label": "math.sqrt",
                    "Kind": 1,
                    "documentation": "math.sqrt (x),返回 x 的平方根。 （你也可以使用乘方 x^0.5 来计算这个值。）",
                    "insertText": "math.sqrt( {{x}})"
                },
                {
                    "label": "math.tan",
                    "Kind": 1,
                    "documentation": "math.tan (x),返回 x 的正切值（假定参数是弧度）",
                    "insertText": "math.tan({{x}} )"
                },
                {
                    "label": "math.tointeger",
                    "Kind": 1,
                    "documentation": "math.tointeger(x),如果 x 可以转换为一个整数， 返回该整数。 否则返回 nil",
                    "insertText": "math.tointeger({{x}})"
                },
                {
                    "label": "math.type",
                    "Kind": 1,
                    "documentation": "math.type (x),如果 x 是整数，返回 integer， 如果它是浮点数，返回 float， 如果 x 不是数字，返回 nil",
                    "insertText": "math.type({{x}})"
                },
                {
                    "label": "math.log",
                    "Kind": 1,
                    "documentation": "math.log (x [, base]),返回以指定底的 x 的对数。 默认的 base 是 e （因此此函数返回 x 的自然对数）",
                    "insertText": "math.log( {{x}})"
                },
                {
                    "label": "math.max",
                    "Kind": 1,
                    "documentation": "math.max (x, ...),返回参数中最大的值， 大小由 Lua 操作 < 决定。 (integer/float)",
                    "insertText": "math.max({{}})"
                },
                {
                    "label": "math.min",
                    "Kind": 1,
                    "documentation": "math.min (x, ...),返回参数中最小的值， 大小由 Lua 操作 < 决定。 (integer/float)",
                    "insertText": "math.min({{}})"
                },
                {
                    "label": "math.fmod",
                    "Kind": 1,
                    "documentation": "math.fmod(x,y),返回 x 除以 y，将商向零圆整后的余数。 (integer/float)",
                    "insertText": "math.fmod( {{x}},{{y}} )"
                },
                 {
                     "label": "math.pow",
                     "Kind": 1,
                     "documentation": "math.pow(x,y),计算x的y次幂。 (integer/float,integer)",
                     "insertText": "math.pow({{x}},{{y}})"
                 },
                {
                    "label": "os",
                    "Kind": 13,
                    "documentation": "os object",
                    "insertText": ""
                },
                {
                    "label": "os.clock",
                    "Kind": 1,
                    "documentation": "os.clock(),返回程序使用的按秒计 CPU 时间的近似值",
                    "insertText": "os.clock()"
                },
                {
                    "label": "os.date",
                    "Kind": 1,
                    "documentation": "os.date ([format [, time]]),返回一个包含日期及时刻的字符串或表.格式化方法取决于所给字符串 format.如果提供了 time 参数， 格式化这个时间 （这个值的含义参见 os.time 函数）。 否则，date 格式化当前时间",
                    "insertText": "os.date({{format}})"
                },
                {
                    "label": "os.time",
                    "Kind": 1,
                    "documentation": "os.time ([table]),当不传参数时，返回当前时刻。 如果传入一张表，就返回由这张表表示的时刻。 这张表必须包含域 year，month，及 day； 可以包含有　hour （默认为 12 ）， min （默认为 0）， sec （默认为 0），以及 isdst （默认为 nil）。 关于这些域的详细描述，参见 os.date 函数",
                    "insertText": "os.time({{table}})"
                },
                {
                    "label": "os.difftime",
                    "Kind": 1,
                    "documentation": "os.difftime (t2, t1),返回以秒计算的时刻 t1 到 t2 的差值。 （这里的时刻是由 os.time 返回的值）。 在 POSIX，Windows，和其它一些系统中，这个值就等于 t2-t1",
                    "insertText": "os.difftime({{t2}},{{t1}})"
                },
                {
                    "label": "os.execute",
                    "Kind": 1,
                    "documentation": "os.execute ([command]) ,这个函数等价于 ISO C 函数 system。 它调用系统解释器执行 command。 如果命令成功运行完毕，第一个返回值就是 true， 否则是 nil otherwise。 在第一个返回值之后，函数返回一个字符串加一个数字.",
                    "insertText": "os.execute('{{command}}')"
                },
                {
                    "label": "os.exit",
                    "Kind": 1,
                    "documentation": "os.exit ([code [, close]]) ,调用 ISO C 函数 exit 终止宿主程序。 如果 code 为 true， 返回的状态码是 EXIT_SUCCESS； 如果 code 为 false， 返回的状态码是 EXIT_FAILURE； 如果 code 是一个数字， 返回的状态码就是这个数字。 code 的默认值为 true,如果第二个可选参数 close 为真， 在退出前关闭 Lua 状态机",
                    "insertText": "os.exit('{{code}}')"
                }
            ];
        }
    });
}
