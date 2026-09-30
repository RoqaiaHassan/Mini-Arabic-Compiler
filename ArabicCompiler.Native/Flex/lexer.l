%option noyywrap
%option yylineno
%option nounput
%option noinput
%option nounistd
%option 8bit

%{
#include "../Bison/parser.tab.h"

#include <stdio.h>
#include <stdlib.h>
#include <string.h>

#ifdef _MSC_VER
#include <io.h>

#define strdup _strdup
#define fileno _fileno
#define isatty _isatty

#endif
%}


DIGIT       [0-9]

ARABIC      (\xD8[\xA1-\xBA]|\xD9[\x81-\x8A])

IDENT       {ARABIC}({ARABIC}|{DIGIT})*

INTEGER     {DIGIT}+

REAL        {DIGIT}+"."{DIGIT}+

STRING      \"[^\"\r\n]*\"

CHARACTER   "’"({ARABIC}|[\x20-\x7E])"‘"

WHITESPACE  [ \t\r\n]+


%%


"برنامج"        { return PROGRAM; }

"ثابت"          { return CONST; }

"نوع"           { return TYPE; }

"قائمة"         { return LIST; }

"من"            { return OF; }

"سجل"           { return RECORD; }

"متغير"         { return VAR; }

"اجراء"         { return PROCEDURE; }
"إجراء"         { return PROCEDURE; }

"بالقيمة"       { return BY_VALUE; }

"بالمرجع"       { return BY_REFERENCE; }


"صحيح"          { return INT_TYPE; }

"حقيقي"         { return REAL_TYPE; }

"منطقي"         { return BOOL_TYPE; }

"حرفي"          { return CHAR_TYPE; }

"خيط"[ \t]+"رمزي"     { return STRING_TYPE; }
"خيط_رمزي"      { return STRING_TYPE; }


"اقرا"          { return READ; }

"اطبع"          { return PRINT; }


"اذا"           { return IF; }
"إذا"           { return IF; }

"فان"           { return THEN; }
"فإن"           { return THEN; }


"؛"[ \t\r\n]*"والا"    { return SEMI_ELSE; }


"والا"          { return ELSE; }
"وإلا"          { return ELSE; }


"كرر"           { return FOR; }

"الى"           { return TO; }
"إلى"           { return TO; }

"اضف"           { return STEP; }


"طالما"         { return WHILE; }

"استمر"         { return DO; }


"اعد"           { return REPEAT; }
"أعد"           { return REPEAT; }

"حتى"           { return UNTIL; }


"صح"            { return TRUE; }

"خطأ"           { return FALSE; }


"<="            { return LE; }

">="            { return GE; }

"=="            { return EQ; }

"!="            { return NE; }

"<"             { return LT; }

">"             { return GT; }


"||"            { return OR; }

"&&"            { return AND; }


"+"             { return PLUS; }

"-"             { return MINUS; }


"^"             { return POWER; }

"*"             { return MUL; }


"/"             { return REAL_DIV; }

"\\"            { return INT_DIV; }

"%"             { return MOD; }


"!"             { return NOT; }


"="             { return ASSIGN; }


"."             { return DOT; }

":"             { return COLON; }

"؛"             { return SEMICOLON; }

","             { return COMMA; }


"{"             { return LBRACE; }

"}"             { return RBRACE; }


"["             { return LBRACKET; }

"]"             { return RBRACKET; }


"("             { return LPAREN; }

")"             { return RPAREN; }


{REAL}          { yylval.text = strdup(yytext); return REAL_LITERAL; }


{INTEGER}       { yylval.text = strdup(yytext); return INTEGER_LITERAL; }


{STRING}        { yylval.text = strdup(yytext); return STRING_LITERAL; }


{CHARACTER}     { yylval.text = strdup(yytext); return CHAR_LITERAL; }


{IDENT}         { yylval.text = strdup(yytext); return IDENTIFIER; }


{WHITESPACE}    { }


.               { return LEXICAL_ERROR; }


%%