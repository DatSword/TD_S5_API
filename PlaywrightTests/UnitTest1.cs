namespace PlaywrightTests;

[TestClass]
public class UnitTest1
{
    [TestMethod]
    public void TestMethod1()
    {
        //Quoi que je fasse, le package refuse de se t�l�charger, et la seule fois o� le package �tait
        //consid�r� comme "install�" (apr�s avoir ajouter explicitement le package dans le csproj)
        //la solution refusait de build.
        //C'est pas que la build rapportait une erreur, la solution buildait dans le vide avec un chargement infini
        //Je m'excuse pour la g�ne occasionn�...
    }
}