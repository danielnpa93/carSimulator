import { Box, Button, FormControl, MenuItem, Select } from "@mui/material";
import React from "react";
import { Route } from "types/models";

interface IProps {
  options: Route[];
  onSubmitRace(id: string): void;
  onRemove(id: string): void;
}

export default function SideBar({ options, onSubmitRace, onRemove }: IProps) {
  React.useEffect(() => {});
  const [raceId, setRaceId] = React.useState<string>("");

  const handleSubmit = () => {
    onSubmitRace(raceId);
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
          value={raceId}
          variant="standard"
          onChange={(e) => setRaceId(e.target.value)}
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
          disabled={!raceId}
          onClick={handleSubmit}
          variant="contained"
        >
          Start
        </Button>
        {/* <Button
          variant="contained"
          disabled={!raceId}
          onClick={() => onRemove(raceId)}
        >
          Remover
        </Button> */}
      </FormControl>
    </Box>
  );
}
