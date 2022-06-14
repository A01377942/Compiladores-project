

namespace QuetzalDragon
{

    //Zabdiel
    class Program : Node { }

    class Def_list : Node { }

    class VarDefList : Node
    {
        public int NumberChildrens
        {
            get
            {

                return children.Count;
            }

        }

    }

    class Stmt_List : Node { }
    class Fun_Def : Node
    {
        public int NumberChildrens
        {
            get
            {

                return children.Count;
            }

        }
    }
    class Assign : Node { }

    class IntLiteral : Node { }

    class FUN_CALL : Node
    {

        public bool isStatement
        {
            get;
            set;
        }



    }
    class Expr_List : Node
    {
        public int NumberChildrens
        {
            get
            {

                return children.Count;
            }

        }
    }
    class Identifier : Node { }

    class Stmt_If : Node { }

    class Array : Node { }

    class Stmt_Loop : Node { }



    //--------------------------------
    //Jonathan

    class MULTIPLICATION : Node { }

    class Stmt_Break : Node { }

    class Stmt_Return : Node { }

    class Expr_And : Node { }

    class Expr_Or : Node { }

    class PLUS : Node
    {
        public int NumberChildrens
        {
            get
            {

                return children.Count;
            }

        }
    }


    class DIVISION : Node { }

    class REMINDER : Node { }

    class NOT : Node { }

    class EQUAL_TO : Node { }

    class NOT_EQUAL_TO : Node { }
    class LESS_THAN : Node { }
    class LESS_EQUAL_THAN : Node { }








    //-------------------------
    //Emiliano

    class GREATHER_THAN : Node { }
    class GREATHER_EQUAL_THAN : Node { }
    class Empty : Node { }
    class Stmt_Incr : Node { }

    class Stmt_Decr : Node { }

    class Else_If_List : Node
    {
        public int NumberChildrens
        {
            get
            {

                return children.Count;
            }

        }
    }

    class Elif : Node { }

    class Else : Node
    {
        public int NumberChildrens
        {
            get
            {

                return children.Count;
            }

        }
    }

    class Op_Unary : Node { }


    class Boolean : Node { }

    class Character : Node { }

    class String : Node { }

    class SUBSTRACTION : Node
    {
        public int NumberChildrens
        {
            get
            {

                return children.Count;
            }

        }
    }
}