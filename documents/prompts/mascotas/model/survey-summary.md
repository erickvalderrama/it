# Thesis survey "Mascotas en tu casa" — summary and question bank

Source: `documents/tesis/Mascotas en tu casa.csv.zip`, a Google Forms export used in the owner's master's thesis. 152 responses, 34 columns. Summarized on 11 November 2026.

**Privacy:** the raw file contains one email address per respondent. Do not copy the raw file into the application repository, fixtures, or training data. Use only this anonymized summary and the question bank. The CSV is encoded as Windows-1252/Latin-1; read it with that encoding.

## Respondents

- Countries: Colombia 123, Chile 10, Spain 7, Ecuador 5, Peru 2, others 5.
- Gender: female 95, male 56, other 1. Ages from 18; most frequent 25–33.
- Household: 3–4 people 74, 2 people 33, 5 or more 31, alone 14.
- Housing: house with garden or yard 64, apartment without garden 63, house without garden 16, other 9.
- Children: none 83, ages 5–12: 32, under 5: 20, over 12: 17.
- Activity: moderate 68, sedentary 38, active 31, very active 15.
- Current pet: yes 110, no 42. Of those without a pet, 9 would adopt and 33 would not.

## Signals relevant to Petyei

| Question | Result | Use |
| --- | --- | --- |
| Personalized pet recommendations based on answers | Yes 102 of 119 (86 %) | Supports the compatibility model and questionnaire. |
| Notifications about matching pets | Email 53, in-app 29, no 37 of 119 | Supports saved preferences and match alerts; offer both channels. |
| Blockchain adoption certificate | Yes 39, unsure 47, no 33 of 119 | Supports dropping blockchain; a regular certificate is enough. |
| Pay a fee for a blockchain certificate | Yes 23, unsure 47, no 49 of 119 | Low willingness to pay; do not monetize certificates. |
| Where owners got their pet | Friends or family 65, shelter 23, breeder 22 | Most adoptions are informal today; the listing flow must be simple for individuals. |
| Most important adoption factor (9 respondents) | Health 4, compatibility with other animals 3, adaptation 2 | Include health status and compatibility with other pets in the pet profile. |

**Limits.** This is a convenience sample of 152 people, mostly from the thesis author's network. It has no adoption outcomes, so it cannot train or validate the model. Use it to design questions, define options, build synthetic fixtures, and set the initial rule-based baseline. Only 9 respondents answered the adoption-preference block.

## Question bank for the questionnaires

Structured options taken from the survey, with English keys and Spanish labels. Store question versions and option keys; render labels through translations.

### Adopter questionnaire

| Key | Question (ES) | Options (key → ES label) |
| --- | --- | --- |
| `householdSize` | ¿Cuántas personas viven en su hogar? | `alone` Vivo solo/a · `two` 2 personas · `threeFour` 3–4 personas · `fivePlus` 5 o más |
| `activityLevel` | ¿Cuál es su nivel de actividad física? | `sedentary` Sedentario · `moderate` Moderado · `active` Activo · `veryActive` Muy activo |
| `housingType` | ¿En qué tipo de vivienda reside? | `houseWithYard` Casa con jardín o patio · `houseNoYard` Casa sin jardín · `apartmentNoYard` Apartamento sin jardín · `other` Otro |
| `children` | ¿Hay niños en su hogar? | `none` No · `under5` Menores de 5 · `age5to12` Entre 5 y 12 · `over12` Mayores de 12 (multiple choice) |
| `otherPets` | ¿Hay otras mascotas en su hogar? | `none` · `dogs` · `cats` · `birds` · `fish` · `other` (multiple choice) |
| `preferredSpecies` | ¿Qué tipo de mascota le gustaría adoptar? | From the species catalog |
| `preferredSize` | Tamaño deseado | `small` Pequeño · `medium` Medio · `large` Grande · `any` Indiferente |
| `preferredCoat` | Pelaje deseado | `short` Corto · `long` Largo · `none` Sin pelaje · `any` Indiferente |
| `preferredAgeStage` | Edad deseada | `baby` Cachorro/Bebé · `young` Joven · `adult` Adulto · `senior` Senior · `any` |
| `preferredPersonality` | Personalidad deseada | `calm` Tranquilo · `energetic` Energético · `friendly` Amable · `reserved` Reservado |
| `travelWillingness` | Ubicación para adoptar | `nearby` Cerca de mi ubicación · `otherCity` Dispuesto a viajar |
| `priorityFactor` | ¿Qué factor es más importante? | `health` Estado de salud · `petCompatibility` Compatibilidad con otros animales · `adaptation` Comodidad en la adaptación |

The survey also asked age, gender, and country. Do not use gender as a matching feature. Use age only to verify the adult requirement and location from the account and DIVIPOLA catalog.

### Pet questionnaire

| Key | Question (ES) | Options |
| --- | --- | --- |
| `species`, `breed` | Especie y raza | Species and breed catalog |
| `size` | Tamaño | `small` · `medium` · `large` |
| `coat` | Pelaje | `short` · `long` · `none` |
| `ageStage` | Edad | `baby` · `young` · `adult` · `senior` |
| `personality` | Personalidad | `calm` · `energetic` · `friendly` · `reserved` (the survey had both "Energético" and "Enérgico"; merge them) |
| `goodWithChildren` | ¿Convive bien con niños? | `yes` · `no` · `unknown` (proposed; not in the survey) |
| `goodWithPets` | ¿Convive con otros animales? | `dogs` · `cats` · `other` · `no` · `unknown` (proposed, from the priority factor) |
| `healthStatus` | Estado de salud | Vaccinated, sterilized, special care (proposed, from the priority factor) |
| `spaceNeeds` | Espacio que necesita | `apartmentOk` · `needsYard` (proposed) |
