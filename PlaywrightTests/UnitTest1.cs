namespace PlaywrightTests;

[TestClass]
public class UnitTest1
{
    [TestMethod]
    public void TestMethod1()
    {
        //Quoi que je fasse, le package refuse de se télécharger, et la seule fois où le package était
        //considéré comme "installé" (après avoir ajouter explicitement le package dans le csproj)
        //la solution refusait de build.
        //C'est pas que la build rapportait une erreur, la solution buildait dans le vide avec un chargement infini
        //Je m'excuse pour la gêne occasionné...
    }
}