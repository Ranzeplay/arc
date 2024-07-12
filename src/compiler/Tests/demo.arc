decl func main()[number]
{
    call external();
    return 0;
}

decl func external()[number]
{
    decl var std::dynamic num1;
    decl var std::dynamic num2;
    num1 = 1;
    num2 = 2;

    if(num1 <> num2)
    {
        num1 = num1 + 500;
    }
    else
    {
        num2 = 1024;
    }

    while(num1 < 200)
    {
        num2 = num2 + 300;
    }

    return num1;
}