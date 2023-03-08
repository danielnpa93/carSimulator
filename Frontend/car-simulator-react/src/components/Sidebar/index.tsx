import {
  Box,
  Button,
  FormControl,
  InputLabel,
  MenuItem,
  Select,
} from "@mui/material";
import React from "react";
import { Route } from "types/models";

interface IProps {
  options: Route[];
  onSubmitRace(id: string): void;
}

export default function SideBar({ options, onSubmitRace }: IProps) {
  React.useEffect(() => {});
  const [race, setRace] = React.useState<string>("");

  const handleSubmit = () => {
    onSubmitRace(race);
  };

  return (
    <Box
      sx={{
        width: "100%",
        height: "100%",
        backgroundColor: "#242526",
        padding: "40px 20px",
      }}
    >
      <FormControl fullWidth>
        {/* <InputLabel id="demo-simple-select-label">Select a race</InputLabel> */}
        <Select
          displayEmpty
          value={race}
          variant="standard"
          onChange={(e) => setRace(e.target.value)}
        >
          <MenuItem style={{ display: "none" }} key={0} value={""}>
            <em>Select a race</em>
          </MenuItem>
          {options.map((o, i) => (
            <MenuItem key={i + 1} value={o.id}>
              {o.title}
            </MenuItem>
          ))}
        </Select>
        <Button
          style={{ marginTop: "30px" }}
          disabled={!race}
          onClick={handleSubmit}
          variant="contained"
        >
          Start
        </Button>
      </FormControl>
    </Box>
  );
}
