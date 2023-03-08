import { Grid, ThemeProvider, CssBaseline } from "@mui/material";
import Sidebar from "components/Sidebar";
import React, { useEffect } from "react";
import { Route } from "types/models";
import { theme } from "theme";
import Map from "components/Map";

const API_URL = process.env.REACT_APP_API_URL;

export default function App() {
  var [runnigRaces, setRunnigRaces] = React.useState<string[]>([]);
  var [racesOptions, setRacesOptions] = React.useState<Route[]>([]);

  useEffect(() => {
    fetch(`${API_URL}`)
      .then((data) => data.json())
      .then(({ data }) => setRacesOptions(data));
  }, []);

  const handleSubmitRace = (id: string) => {
    var newRaces = [...runnigRaces, id];
    setRunnigRaces((prev) => [...prev, id]);

    console.log(newRaces);
  };

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
          <Sidebar options={racesOptions} onSubmitRace={handleSubmitRace} />
        </Grid>
        <Grid item xs={12} sm={9}>
          <Map />
        </Grid>
      </Grid>
    </ThemeProvider>
  );
}
