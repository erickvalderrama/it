# Catalog seed data

Versioned inputs for the catalog import jobs. Change a file, update its source version, and run the import again; imports update records by code and never delete them.

| File | Source | License |
| --- | --- | --- |
| `divipola/colombia-departments.csv`, `divipola/colombia-divipola-localities.csv` | DANE, DIVIPOLA Códigos municipios (dataset `gdxc-w37w`), cut 2024-12-30, downloaded 2026-11-09. Details in `documents/prompts/mascotas/data/README.md`. | CC BY-SA 4.0 |
| `species-breeds.csv` | Owner's master's thesis spreadsheet (`documents/tesis/Especies-Razas.xlsx`), with English names added, "Cobaya (Cuyo)" and "Conejillo de Indias" merged as `guinea_pig`, and "Cerdo Barrigón" merged into `vietnamese_potbellied`. | Project data |

Source versions: DIVIPOLA `DANE-2024-12-30`; species and breeds `thesis-2024-08-20.1`.
