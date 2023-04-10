import { Grid, ThemeProvider, CssBaseline } from "@mui/material";
import Sidebar from "components/Sidebar";
import React, { useCallback, useEffect } from "react";
import { Route } from "types/models";
import { theme } from "theme";
import Map from "components/Map";
import { useSnackbar } from "notistack";

const API_URL = process.env.REACT_APP_API_URL;

export default function App() {
  var [runnigRaces, setRunnigRaces] = React.useState<string[]>([]);
  var [racesOptions, setRacesOptions] = React.useState<Route[]>([]);
  const { enqueueSnackbar } = useSnackbar();

  useEffect(() => {
    fetch(`${API_URL}`)
      .then((data) => data.json())
      .then(({ data }) => setRacesOptions(data));
  }, []);

  const handleSubmitRace = useCallback(
    (id: string) => {
      if (!id) {
        return;
      }

      if (runnigRaces.includes(id)) {
        enqueueSnackbar("Route was already started", {
          variant: "error",
        });
        return;
      }
      setRunnigRaces((prev) => [...prev, id]);
    },
    [runnigRaces, enqueueSnackbar]
  );

  const handleFinishRace = useCallback(
    (id: string) => {
      setRunnigRaces((pre) => pre.filter((r) => r !== id));

      enqueueSnackbar("Finished Route", {
        variant: "success",
      });
    },
    [enqueueSnackbar]
  );

  // const handleUpdateRace = useCallback(
  //   (e: { latitude: number; longitude: number; id: string }) => {
  //     var races = racesOptions.map((r) => {
  //       if (r.id !== e.id) {
  //         return r;
  //       }

  //       return {
  //         ...r,
  //         currentPosition: {
  //           latitude: e.latitude,
  //           longitude: e.longitude,
  //         },
  //       };
  //     });

  //     setRacesOptions((prev) => races);
  //   },
  //   [runnigRaces]
  // );

  return (
    <ThemeProvider theme={theme}>
      <CssBaseline />
      <Grid
        sx={{
          height: "100%",
        }}
        container
      >
        <Grid item xs={12} sm={3}>
          <Sidebar
            options={racesOptions}
            onSubmitRace={handleSubmitRace}
            onRemove={(id) =>
              setRunnigRaces((pre) => pre.filter((x) => x !== id))
            }
          />
        </Grid>
        <Grid item xs={12} sm={9}>
          <Map
            onFinishRace={handleFinishRace}
            // updateRaces={handleUpdateRace}
            races={racesOptions.filter((r) => runnigRaces.includes(r.id))}
          />
        </Grid>
      </Grid>
    </ThemeProvider>
  );
}
