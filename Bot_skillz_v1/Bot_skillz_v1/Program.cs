using PenguinGame;
//The best bot
namespace MyBot
{
    /// <summary>
    /// This is an example for a bot.
    /// </summary>
    public class TutorialBot : ISkillzBot
    {
        /// <summary>
        /// Makes the bot run a single turn.                                    |ver: stage1C + passsive overwhelm|
        /// </summary>
        /// <param name="game">the current game state</param>
        public void DoTurn(Game game)
        {
            if(game.GetMyIcepitalIcebergs().Length < 2){
            bool stage1C = false, emergency = false, overwhelmGO = true, upgradeGO = true;
            Iceberg[] myIcebergs = game.GetMyIcebergs();
            Iceberg myIcepital = game.GetMyIcepitalIcebergs()[0], enemyIcepital = game.GetEnemyIcepitalIcebergs()[0];
            PenguinGroup[] myPenGroups = game.GetMyPenguinGroups(), enePenGroups = game.GetEnemyPenguinGroups();
            myIcebergs = game.GetMyIcebergs();
            int enemyIcepitalLevel = game.GetEnemyIcepitalIcebergs()[0].Level, enemyIcepitalPenguins = game.GetEnemyIcepitalIcebergs()[0].PenguinAmount;
                 // sends penguins to nutral iceberg
            // if(myIcebergs.Length <= 1 && game.Turn<16 && game.GetNeutralIcebergs().Length>0 ){
            //     if(myIcepital.PenguinAmount > game.GetNeutralIcebergs()[0].PenguinAmount){
            //       myIcepital.SendPenguins(game.GetNeutralIcebergs()[0], myIcepital.PenguinAmount);  
            //     }
            // }
           // else{
            if(myPenGroups.Length > 0 && game.Turn>25 && game.Turn<250){
                for(int i = 0; i < myPenGroups.Length; i++){
                    if(myPenGroups[i].Source == myIcepital)
                        myPenGroups[i].Accelerate();
                }
            }
            int totalPenAmount = 0;
            for(int i = 0; i < enePenGroups.Length; i++){
                if(enePenGroups[i].Destination == myIcepital && enePenGroups[i].PenguinAmount > myIcepital.Level)
                    totalPenAmount += enePenGroups[i].PenguinAmount;
                if(enePenGroups[i].Destination == myIcepital && enePenGroups[i].Source == enemyIcepital && enePenGroups[i].PenguinAmount > 10)
                    overwhelmGO = false;
                if (totalPenAmount + 10 > myIcepital.PenguinAmount)
                    upgradeGO = false;
                
            }
            //if (enemy_forces.Length > 10)
            //  myIcepital.SendPenguins(game.GetEnemyIcepitalIcebergs()[0], myIcepital.PenguinAmount);
            //for(int Ceme = 0; Ceme)
            /*if(game.Turn>250){
                myIcepital.SendPenguins(game.GetEnemyIcepitalIcebergs()[0], myIcepital.PenguinAmount);
            }*/
            //else{
            if (myIcebergs.Length > 2 && myIcepital.Level == 4 && (!((enemyIcepitalPenguins + 30) * 2 <= myIcepital.PenguinAmount))) // passivly sends penguins from the icepital to the cloneberg
                myIcepital.SendPenguins(game.GetCloneberg(), myIcepital.Level);
            else if ((enemyIcepitalPenguins + 30) * 2 <= myIcepital.PenguinAmount){ //overwheliming the enemy capital
                if (enemyIcepitalPenguins < 40 && overwhelmGO == true)
                    myIcepital.SendPenguins(game.GetEnemyIcepitalIcebergs()[0], myIcepital.PenguinAmount);
                else{
                    if (overwhelmGO = true)
                        myIcepital.SendPenguins(game.GetEnemyIcepitalIcebergs()[0], enemyIcepitalLevel * 10 + enemyIcepitalPenguins * 3);
                }
            }
            if((game.Turn < 35 || myIcepital.PenguinAmount > 100) && stage1C == false){ //maxes capital level. stage1: code completed
                if(upgradeGO == true)
                    myIcepital.Upgrade();
                if (myIcepital.Level == 4)
                    stage1C = true;
            }
            if (stage1C == true && myIcebergs.Length <= 2 && myIcepital.PenguinAmount > game.GetEnemyIcepitalIcebergs()[0].PenguinAmount + 11) {
                if (myIcebergs.Length <= 2){
                    Iceberg destination = game.GetNeutralIcebergs()[0];
                    myIcepital.SendPenguins(destination, 11);
                }
                
            }
           // }
          //}
         }
           //Sends peguins from nutral to Cloneberg+ if number is very big sends to enemy
        //  for (int i = 0; i <  game.GetMyIcebergs().Length; i++)
        // {
        //         // The iceberg we are going over.
        //         Iceberg myIceberg =  game.GetMyIcebergs()[i];
        //         if(!myIceberg.IsIcepital && game.GetMyIcebergs()[i].PenguinAmount>game.GetEnemyIcepitalIcebergs()[0].PenguinAmount+40 && game.GetEnemyIcepitalIcebergs().Length > 0){
        //             game.GetMyIcebergs()[i].SendPenguins(game.GetEnemyIcepitalIcebergs()[0], game.GetMyIcebergs()[i].PenguinAmount);
        //         }
        //         else if(!myIceberg.IsIcepital )
        //         {
        //               game.GetMyIcebergs()[i].SendPenguins(game.GetCloneberg(), game.GetMyIcebergs()[i].Level);
        //         }
        // }
      }
    }
}
