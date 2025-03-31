using ContradictiveGames.Player;
using UnityEditor;
using UnityEngine;

namespace ContradictiveGames.EditorViews
{
    [CustomEditor(typeof(PlayerClassData))]
    public class PlayerClassView : Editor
    {
        private bool showPrimaryAtk = true;
        private bool showSecondaryAtk = true;

        public override void OnInspectorGUI()
        {
            serializedObject.Update();


            var script = (PlayerClassData)target;

            EditorGUILayout.LabelField("Class UI", EditorStyles.boldLabel);
            script.ClassName = EditorGUILayout.TextField("Class Name", script.ClassName);
            script.ClassDescription = EditorGUILayout.TextField("Class Description", script.ClassDescription, GUILayout.Height(50));
            script.ClassIcon = (Sprite)EditorGUILayout.ObjectField("Class Icon", script.ClassIcon, typeof(Sprite), false);


            EditorGUILayout.Space(20);

            EditorGUILayout.LabelField("Combat Settings", EditorStyles.boldLabel);
            script.MaxHealth = EditorGUILayout.IntField("Max Health", script.MaxHealth);
            EditorGUILayout.Space();
            
            showPrimaryAtk = EditorGUILayout.Foldout(showPrimaryAtk, "Primary Attack", true);
            if(showPrimaryAtk) DrawAttackEditorSections(ref script.PrimaryAttack);
            
            EditorGUILayout.Space();

            showSecondaryAtk = EditorGUILayout.Foldout(showSecondaryAtk, "Secondary Attack", true);
            if(showSecondaryAtk) DrawAttackEditorSections(ref script.SecondaryAttack);
            

            
        }

        private void DrawAttackEditorSections(ref Attack attack){

            if(attack == null) {
                EditorGUILayout.HelpBox("No attack has been created", MessageType.Warning);
                return;
            }

            EditorGUILayout.BeginVertical(GUI.skin.box);
            GUI.backgroundColor = new Color(0.15f, 0.15f, 0.15f);

            attack.AttackName = EditorGUILayout.TextField("Attack Name", attack.AttackName);
            attack.AttackIcon = (Sprite)EditorGUILayout.ObjectField("Attack Icon", attack.AttackIcon, typeof(Sprite), false);

            EditorGUILayout.LabelField("Base Stats", EditorStyles.boldLabel);

            attack.BaseDamageMin = EditorGUILayout.FloatField("Base Damage Min", attack.BaseDamageMin);
            attack.BaseDamageMax = EditorGUILayout.FloatField("Base Damage Max", attack.BaseDamageMax);
            attack.AttackCooldown = EditorGUILayout.FloatField("Cooldown", attack.AttackCooldown);

            attack.attackType = (AttackType)EditorGUILayout.EnumPopup("Attack Type", attack.attackType);

            EditorGUILayout.Space();

            switch(attack.attackType){
                case AttackType.Melee:
                    EditorGUILayout.LabelField("Melee Attack Settings", EditorStyles.boldLabel);
                    attack.SwingRadius = EditorGUILayout.FloatField("Swing Radius", attack.SwingRadius);
                    attack.SwingLength = EditorGUILayout.FloatField("Swing Length", attack.SwingLength);
                    attack.SwingSpeed = EditorGUILayout.FloatField("Swing Speed", attack.SwingSpeed);
                    break;


                case AttackType.Ranged:
                    EditorGUILayout.LabelField("Ranged Attack Settings", EditorStyles.boldLabel);
                    attack.RangedAttackType = (RangedAttackType)EditorGUILayout.EnumPopup("Ranged Attack Type", attack.RangedAttackType);
                    attack.projectilePrefab = (GameObject)EditorGUILayout.ObjectField("Projectile Prefab", attack.projectilePrefab, typeof(GameObject), false);
                    attack.ProjectileSpeed = EditorGUILayout.FloatField("Projectile Speed", attack.ProjectileSpeed);
                    
                    break;
            }


            GUI.backgroundColor = Color.white;
            EditorGUILayout.EndVertical();
        }
    }
}
